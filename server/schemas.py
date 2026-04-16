from pydantic import BaseModel
from typing import Optional, List
from datetime import datetime

class TaskBase(BaseModel):
    title: str
    description: Optional[str] = ""
    due_date: datetime
    created_date: Optional[datetime] = None
    completion_date: Optional[datetime] = None
    is_completed: bool = False
    is_important: bool = False
    notes: Optional[str] = ""
    sync_id: str

class TaskCreate(TaskBase):
    pass

class TaskResponse(TaskBase):
    id: int
    user_id: int
    
    class Config:
        from_attributes = True

class UserBase(BaseModel):
    username: str
    email: str

class UserCreate(UserBase):
    password: str
    recovery_word: str

class UserLogin(BaseModel):
    username: str
    password: str

class UserRecovery(BaseModel):
    username: str
    recovery_word: str
    new_password: str

class UserResponse(UserBase):
    id: int
    created_at: datetime
    
    class Config:
        from_attributes = True

class SyncRequest(BaseModel):
    tasks: List[TaskCreate]
    last_sync: Optional[datetime] = None

class SyncResponse(BaseModel):
    tasks: List[TaskResponse]
    sync_time: datetime