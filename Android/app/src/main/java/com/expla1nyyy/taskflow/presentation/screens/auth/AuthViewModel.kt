package com.expla1nyyy.taskflow.presentation.screens.auth

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.expla1nyyy.taskflow.data.repository.AuthRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.map
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class AuthViewModel @Inject constructor(
    private val authRepository: AuthRepository
) : ViewModel() {

    private val _uiState = MutableStateFlow(AuthUiState())
    val uiState: StateFlow<AuthUiState> = _uiState.asStateFlow()

    val isLoggedIn = authRepository.tokenFlow.map { token -> token != null }

    fun login(username: String, password: String) {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(isLoading = true, error = null)
            val result = authRepository.login(username, password)
            result.onSuccess { token ->
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    isSuccess = true,
                    token = token
                )
            }.onFailure { exception ->
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    error = exception.message ?: "Ошибка входа"
                )
            }
        }
    }

    fun register(username: String, email: String, password: String, recoveryWord: String) {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(isLoading = true, error = null)
            val result = authRepository.register(username, email, password, recoveryWord)
            result.onSuccess {
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    isRegisterSuccess = true
                )
            }.onFailure { exception ->
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    error = exception.message ?: "Ошибка регистрации"
                )
            }
        }
    }

    fun recoverPassword(username: String, recoveryWord: String, newPassword: String) {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(isLoading = true, error = null)
            val result = authRepository.recoverPassword(username, recoveryWord, newPassword)
            result.onSuccess {
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    isRecoverSuccess = true
                )
            }.onFailure { exception ->
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    error = exception.message ?: "Ошибка восстановления"
                )
            }
        }
    }

    fun resetState() {
        _uiState.value = AuthUiState()
    }

    data class AuthUiState(
        val isLoading: Boolean = false,
        val isSuccess: Boolean = false,
        val isRegisterSuccess: Boolean = false,
        val isRecoverSuccess: Boolean = false,
        val error: String? = null,
        val token: String? = null
    )
}