from passlib.context import CryptContext
from jose import JWTError, jwt
from datetime import datetime, timedelta
from typing import Optional
import os

# Секретный ключ для JWT
SECRET_KEY = "your-secret-key-change-this-in-production"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_HOURS = 24 * 7  # Токен живет 7 дней

# Настройка passlib с явным указанием бэкенда
pwd_context = CryptContext(
    schemes=["bcrypt"],
    deprecated="auto",
    bcrypt__rounds=12
)

def verify_password(plain_password, hashed_password):
    """Проверка пароля с обработкой ошибок"""
    try:
        print("Verifying password...")
        if isinstance(plain_password, str):
            plain_password = plain_password.encode('utf-8')
        
        if len(plain_password) > 72:
            plain_password = plain_password[:72]
            
        result = pwd_context.verify(plain_password, hashed_password)
        print(f"Password verification result: {result}")
        return result
    except Exception as e:
        print(f"Error verifying password: {e}")
        return False

def get_password_hash(password):
    """Хеширование пароля с обработкой длины"""
    try:
        print("Hashing password...")
        if isinstance(password, str):
            password = password.encode('utf-8')
        
        if len(password) > 72:
            print(f"Password too long ({len(password)} bytes), truncating to 72 bytes")
            password = password[:72]
            
        hashed = pwd_context.hash(password)
        print("Password hashed successfully")
        return hashed
    except Exception as e:
        print(f"Error hashing password: {e}")
        if isinstance(password, bytes) and len(password) > 72:
            return pwd_context.hash(password[:72])
        raise

def create_access_token(data: dict, expires_delta: Optional[timedelta] = None):
    to_encode = data.copy()
    if expires_delta:
        expire = datetime.utcnow() + expires_delta
    else:
        expire = datetime.utcnow() + timedelta(hours=ACCESS_TOKEN_EXPIRE_HOURS)
    to_encode.update({"exp": expire})
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    return encoded_jwt

def verify_token(token: str):
    """Проверка токена с подробным логированием"""
    try:
        if not token:
            print("Token is empty")
            return None
            
        # Проверяем формат токена
        parts = token.split('.')
        if len(parts) != 3:
            print(f"Invalid token format: {token[:20]}... (has {len(parts)} parts)")
            return None
            
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
        print(f"Token verified for user: {payload.get('sub')}")
        return payload
    except jwt.ExpiredSignatureError:
        print("Token has expired")
        return None
    except JWTError as e:
        print(f"Token verification error: {e}")
        return None