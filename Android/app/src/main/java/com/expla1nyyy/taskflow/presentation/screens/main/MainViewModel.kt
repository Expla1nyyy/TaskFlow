package com.expla1nyyy.taskflow.presentation.screens.main

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.expla1nyyy.taskflow.data.models.Task
import com.expla1nyyy.taskflow.data.repository.AuthRepository
import com.expla1nyyy.taskflow.data.repository.TaskRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch
import java.util.Date
import java.util.UUID
import javax.inject.Inject

@HiltViewModel
class MainViewModel @Inject constructor(
    private val taskRepository: TaskRepository,
    private val authRepository: AuthRepository
) : ViewModel() {

    private val _activeTasks = MutableStateFlow<List<Task>>(emptyList())
    val activeTasks: StateFlow<List<Task>> = _activeTasks.asStateFlow()

    private val _completedTasks = MutableStateFlow<List<Task>>(emptyList())
    val completedTasks: StateFlow<List<Task>> = _completedTasks.asStateFlow()

    private val _isSyncing = MutableStateFlow(false)
    val isSyncing: StateFlow<Boolean> = _isSyncing.asStateFlow()

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()

    private val _username = MutableStateFlow<String?>(null)
    val username: StateFlow<String?> = _username.asStateFlow()

    init {
        loadTasks()
        loadUsername()
    }

    private fun loadTasks() {
        viewModelScope.launch {
            taskRepository.getActiveTasks().collect { tasks ->
                _activeTasks.value = tasks
            }
        }

        viewModelScope.launch {
            taskRepository.getCompletedTasks().collect { tasks ->
                _completedTasks.value = tasks
            }
        }
    }

    private fun loadUsername() {
        viewModelScope.launch {
            authRepository.usernameFlow.collect { username ->
                _username.value = username
            }
        }
    }

    fun addTask(title: String, description: String, dueDate: Date, isImportant: Boolean) {
        viewModelScope.launch {
            val newTask = Task(
                syncId = UUID.randomUUID().toString(),
                title = title,
                description = description,
                dueDate = dueDate,
                createdDate = Date(),
                isImportant = isImportant
            )
            taskRepository.insertTask(newTask)
        }
    }

    fun updateTask(task: Task) {
        viewModelScope.launch {
            taskRepository.updateTask(task)
        }
    }

    fun deleteTask(task: Task) {
        viewModelScope.launch {
            taskRepository.deleteTask(task)
        }
    }

    fun completeTask(task: Task) {
        viewModelScope.launch {
            task.isCompleted = true
            task.completionDate = Date()
            taskRepository.updateTask(task)
        }
    }

    fun syncWithServer() {
        viewModelScope.launch {
            _isSyncing.value = true
            _error.value = null

            val token = authRepository.getToken()
            if (token != null) {
                val success = taskRepository.syncWithServer(token)
                if (!success) {
                    _error.value = "Ошибка синхронизации"
                }
            } else {
                _error.value = "Не авторизован"
            }

            _isSyncing.value = false
        }
    }

    fun logout() {
        viewModelScope.launch {
            authRepository.logout()
        }
    }

    fun clearError() {
        _error.value = null
    }
}