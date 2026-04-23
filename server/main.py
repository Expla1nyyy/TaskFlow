from fastapi import FastAPI, HTTPException, Depends, status, Header
from fastapi.middleware.cors import CORSMiddleware
from sqlalchemy.orm import Session
from typing import List, Optional
from datetime import datetime
import uuid
import os

from jose import JWTError, jwt

from database import get_db_session, engine
from models import Base, User, Task
from schemas import *
from auth import verify_password, get_password_hash, create_access_token, verify_token, SECRET_KEY, ALGORITHM

# Создание таблиц
Base.metadata.create_all(bind=engine)

app = FastAPI(title="TaskFlow API")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/health")
def health_check():
    return {"status": "healthy", "timestamp": datetime.utcnow().isoformat()}

@app.post("/api/register", response_model=UserResponse)
def register(user: UserCreate, db: Session = Depends(get_db_session)):
    try:
        db_user = db.query(User).filter(
            (User.username == user.username) | (User.email == user.email)
        ).first()
        if db_user:
            raise HTTPException(status_code=400, detail="Username or email already registered")
        
        hashed_password = get_password_hash(user.password)
        
        db_user = User(
            username=user.username,
            email=user.email,
            hashed_password=hashed_password,
            recovery_word=user.recovery_word
        )
        db.add(db_user)
        db.commit()
        db.refresh(db_user)
        
        return db_user
    except Exception as e:
        db.rollback()
        print(f"Registration error: {e}")
        raise HTTPException(status_code=500, detail=f"Registration failed: {str(e)}")

@app.post("/api/login")
def login(user_data: UserLogin, db: Session = Depends(get_db_session)):
    try:
        user = db.query(User).filter(User.username == user_data.username).first()
        if not user or not verify_password(user_data.password, user.hashed_password):
            raise HTTPException(status_code=401, detail="Incorrect username or password")
        
        access_token = create_access_token(data={"sub": user.username})
        
        return {
            "access_token": access_token,
            "token_type": "bearer",
            "expires_in": 24 * 7 * 60 * 60,
            "user": {"id": user.id, "username": user.username, "email": user.email}
        }
    except HTTPException:
        raise
    except Exception as e:
        print(f"Login error: {e}")
        raise HTTPException(status_code=500, detail=f"Login failed: {str(e)}")

@app.post("/api/refresh-token")
def refresh_token(authorization: Optional[str] = Header(None), db: Session = Depends(get_db_session)):
    try:
        token = extract_token(authorization, None)
        if not token:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="No token provided")
        
        try:
            payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM], options={"verify_exp": False})
        except Exception as e:
            print(f"Failed to decode token: {e}")
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid token format")
        
        username = payload.get("sub")
        if not username:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid token payload")
        
        user = db.query(User).filter(User.username == username).first()
        if not user:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="User not found")
        
        new_token = create_access_token(data={"sub": username})
        
        return {"access_token": new_token, "token_type": "bearer", "expires_in": 24 * 7 * 60 * 60}
    except Exception as e:
        print(f"Refresh error: {e}")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Token refresh failed")

@app.post("/api/recover-password")
def recover_password(recovery: UserRecovery, db: Session = Depends(get_db_session)):
    try:
        user = db.query(User).filter(User.username == recovery.username).first()
        if not user:
            raise HTTPException(status_code=404, detail="User not found")
        
        if user.recovery_word != recovery.recovery_word:
            raise HTTPException(status_code=400, detail="Invalid recovery word")
        
        user.hashed_password = get_password_hash(recovery.new_password)
        db.commit()
        
        return {"message": "Password updated successfully"}
    except Exception as e:
        db.rollback()
        print(f"Recovery error: {e}")
        raise HTTPException(status_code=500, detail=f"Password recovery failed: {str(e)}")

@app.post("/api/sync", response_model=SyncResponse)
def sync_tasks(
    sync_data: SyncRequest,
    authorization: Optional[str] = Header(None),
    token: Optional[str] = None,
    db: Session = Depends(get_db_session)
):
    try:
        actual_token = extract_token(authorization, token)
        if not actual_token:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="No token provided")
        
        user = get_current_user(actual_token, db)
        
        existing_tasks = db.query(Task).filter(Task.user_id == user.id).all()
        existing_sync_ids = {task.sync_id: task for task in existing_tasks}
        
        # Создаем словарь для поиска дубликатов по названию и дате
        existing_by_title_date = {}
        for task in existing_tasks:
            key = f"{task.title}_{task.due_date.date()}_{task.due_date.hour}_{task.due_date.minute}"
            existing_by_title_date[key] = task
        
        print(f"=== SYNC DEBUG ===")
        print(f"User: {user.username}")
        print(f"Existing tasks in DB: {len(existing_tasks)}")
        print(f"Tasks from client: {len(sync_data.tasks)}")
        
        tasks_updated = 0
        tasks_created = 0
        tasks_skipped = 0
        
        for client_task in sync_data.tasks:
            # Проверяем по sync_id
            if client_task.sync_id in existing_sync_ids:
                task = existing_sync_ids[client_task.sync_id]
                task.title = client_task.title
                task.description = client_task.description
                task.due_date = client_task.due_date
                task.is_completed = client_task.is_completed
                task.is_important = client_task.is_important
                task.notes = client_task.notes
                task.completion_date = client_task.completion_date
                tasks_updated += 1
                print(f"  -> UPDATED by sync_id: {client_task.title}")
                continue
            
            # Проверяем по названию и дате (поиск дубликатов)
            task_key = f"{client_task.title}_{client_task.due_date.date()}_{client_task.due_date.hour}_{client_task.due_date.minute}"
            
            if task_key in existing_by_title_date:
                # Нашли дубликат - обновляем существующую задачу и присваиваем ей sync_id клиента
                existing_task = existing_by_title_date[task_key]
                existing_task.title = client_task.title
                existing_task.description = client_task.description
                existing_task.due_date = client_task.due_date
                existing_task.is_completed = client_task.is_completed
                existing_task.is_important = client_task.is_important
                existing_task.notes = client_task.notes
                existing_task.completion_date = client_task.completion_date
                # Важно: сохраняем sync_id клиента для будущих синхронизаций
                existing_task.sync_id = client_task.sync_id
                tasks_updated += 1
                print(f"  -> UPDATED by title/date (duplicate found): {client_task.title}")
            else:
                # Создаем новую задачу
                task = Task(
                    user_id=user.id,
                    title=client_task.title,
                    description=client_task.description,
                    due_date=client_task.due_date,
                    created_date=client_task.created_date or datetime.utcnow(),
                    completion_date=client_task.completion_date,
                    is_completed=client_task.is_completed,
                    is_important=client_task.is_important,
                    notes=client_task.notes,
                    sync_id=client_task.sync_id
                )
                db.add(task)
                tasks_created += 1
                print(f"  -> CREATED: {client_task.title}")
        
        db.commit()
        
        print(f"=== SYNC RESULT: {tasks_updated} updated, {tasks_created} created, {tasks_skipped} skipped ===")
        
        updated_tasks = db.query(Task).filter(Task.user_id == user.id).all()
        
        for task in updated_tasks:
            print(f"  Server task after sync: {task.title}, sync_id: {task.sync_id}, due_date: {task.due_date}")
        
        return SyncResponse(
            tasks=updated_tasks,
            sync_time=datetime.utcnow()
        )
        
    except Exception as e:
        db.rollback()
        print(f"Sync error: {e}")
        import traceback
        traceback.print_exc()
        raise HTTPException(status_code=500, detail=f"Sync failed: {str(e)}")
    

@app.get("/api/tasks", response_model=List[TaskResponse])
def get_tasks(authorization: Optional[str] = Header(None), db: Session = Depends(get_db_session)):
    try:
        actual_token = extract_token(authorization, None)
        if not actual_token:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="No token provided")
        
        user = get_current_user(actual_token, db)
        tasks = db.query(Task).filter(Task.user_id == user.id).all()
        return tasks
    except Exception as e:
        print(f"Get tasks error: {e}")
        raise HTTPException(status_code=500, detail=f"Failed to get tasks: {str(e)}")

@app.delete("/api/tasks/{sync_id}")
def delete_task(sync_id: str, authorization: Optional[str] = Header(None), db: Session = Depends(get_db_session)):
    try:
        actual_token = extract_token(authorization, None)
        if not actual_token:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="No token provided")
            
        user = get_current_user(actual_token, db)
        task = db.query(Task).filter(Task.sync_id == sync_id, Task.user_id == user.id).first()
        
        if not task:
            raise HTTPException(status_code=404, detail="Task not found")
        
        db.delete(task)
        db.commit()
        
        return {"message": "Task deleted successfully"}
    except Exception as e:
        db.rollback()
        print(f"Delete error: {e}")
        raise HTTPException(status_code=500, detail=f"Delete failed: {str(e)}")

def extract_token(authorization: Optional[str], token_param: Optional[str]) -> Optional[str]:
    if authorization and authorization.startswith("Bearer "):
        return authorization.replace("Bearer ", "")
    elif token_param:
        return token_param
    return None

def get_current_user(token: str, db: Session):
    payload = verify_token(token)
    if not payload:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid authentication credentials")
    
    username = payload.get("sub")
    if not username:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid token payload")
        
    user = db.query(User).filter(User.username == username).first()
    if not user:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="User not found")
    return user

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)