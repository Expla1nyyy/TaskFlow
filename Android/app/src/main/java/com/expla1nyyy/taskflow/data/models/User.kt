package com.expla1nyyy.taskflow.data.models

import java.util.Date

data class User(
    val id: Int,
    val username: String,
    val email: String
)

data class LoginRequest(
    val username: String,
    val password: String
)

data class LoginResponse(
    val access_token: String,
    val token_type: String,
    val expires_in: Int,
    val user: User
)

data class RegisterRequest(
    val username: String,
    val email: String,
    val password: String,
    val recovery_word: String
)

data class RecoverPasswordRequest(
    val username: String,
    val recovery_word: String,
    val new_password: String
)

data class SyncRequest(
    val tasks: List<Task>,
    val last_sync: Date? = null
)

data class SyncResponse(
    val tasks: List<Task>,
    val sync_time: Date
)