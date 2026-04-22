package com.expla1nyyy.taskflow.data.network

import com.expla1nyyy.taskflow.data.models.*
import retrofit2.http.*

interface ApiService {

    @POST("api/register")
    suspend fun register(@Body request: RegisterRequest): User

    @POST("api/login")
    suspend fun login(@Body request: LoginRequest): LoginResponse

    @POST("api/recover-password")
    suspend fun recoverPassword(@Body request: RecoverPasswordRequest): Map<String, String>

    @POST("api/sync")
    suspend fun syncTasks(
        @Header("Authorization") token: String,
        @Body request: SyncRequest
    ): SyncResponse

    @GET("api/tasks")
    suspend fun getTasks(
        @Header("Authorization") token: String
    ): List<Task>

    @DELETE("api/tasks/{syncId}")
    suspend fun deleteTask(
        @Header("Authorization") token: String,
        @Path("syncId") syncId: String
    ): Map<String, String>
}