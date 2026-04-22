package com.expla1nyyy.taskflow.data.repository

import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import com.expla1nyyy.taskflow.data.models.LoginRequest
import com.expla1nyyy.taskflow.data.models.RegisterRequest
import com.expla1nyyy.taskflow.data.network.ApiService
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.first
import kotlinx.coroutines.flow.map
import javax.inject.Inject
import javax.inject.Singleton

private val Context.dataStore: DataStore<Preferences> by preferencesDataStore(name = "auth")

@Singleton
class AuthRepository @Inject constructor(
    private val context: Context,
    private val apiService: ApiService
) {

    companion object {
        private val TOKEN_KEY = stringPreferencesKey("token")
        private val USERNAME_KEY = stringPreferencesKey("username")
    }

    val tokenFlow: Flow<String?> = context.dataStore.data.map { preferences ->
        preferences[TOKEN_KEY]
    }

    val usernameFlow: Flow<String?> = context.dataStore.data.map { preferences ->
        preferences[USERNAME_KEY]
    }

    suspend fun login(username: String, password: String): Result<String> {
        return try {
            val response = apiService.login(LoginRequest(username, password))
            saveToken(response.access_token)
            saveUsername(username)
            Result.success(response.access_token)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun register(username: String, email: String, password: String, recoveryWord: String): Result<Unit> {
        return try {
            apiService.register(RegisterRequest(username, email, password, recoveryWord))
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun recoverPassword(username: String, recoveryWord: String, newPassword: String): Result<Unit> {
        return try {
            apiService.recoverPassword(
                com.expla1nyyy.taskflow.data.models.RecoverPasswordRequest(
                    username, recoveryWord, newPassword
                )
            )
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    private suspend fun saveToken(token: String) {
        context.dataStore.edit { preferences ->
            preferences[TOKEN_KEY] = token
        }
    }

    private suspend fun saveUsername(username: String) {
        context.dataStore.edit { preferences ->
            preferences[USERNAME_KEY] = username
        }
    }

    suspend fun logout() {
        context.dataStore.edit { preferences ->
            preferences.remove(TOKEN_KEY)
            preferences.remove(USERNAME_KEY)
        }
    }

    suspend fun getToken(): String? {
        return tokenFlow.first()
    }
}