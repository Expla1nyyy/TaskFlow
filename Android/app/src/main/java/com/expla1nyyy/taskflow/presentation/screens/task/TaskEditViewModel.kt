package com.expla1nyyy.taskflow.presentation.screens.task

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.expla1nyyy.taskflow.data.models.Task
import com.expla1nyyy.taskflow.data.repository.TaskRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import java.util.Calendar
import java.util.Date
import java.util.UUID
import javax.inject.Inject

@HiltViewModel
class TaskEditViewModel @Inject constructor(
    private val taskRepository: TaskRepository
) : ViewModel() {

    private val _task = MutableStateFlow<Task?>(null)
    val task: StateFlow<Task?> = _task.asStateFlow()

    private val _title = MutableStateFlow("")
    val title: StateFlow<String> = _title.asStateFlow()

    private val _description = MutableStateFlow("")
    val description: StateFlow<String> = _description.asStateFlow()

    private val _dueDate = MutableStateFlow(Calendar.getInstance().apply {
        add(Calendar.DAY_OF_YEAR, 1)
        set(Calendar.HOUR_OF_DAY, 18)
        set(Calendar.MINUTE, 0)
    }.time)
    val dueDate: StateFlow<Date> = _dueDate.asStateFlow()

    private val _isImportant = MutableStateFlow(false)
    val isImportant: StateFlow<Boolean> = _isImportant.asStateFlow()

    private val _notes = MutableStateFlow("")
    val notes: StateFlow<String> = _notes.asStateFlow()

    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()

    fun loadTask(syncId: String) {
        viewModelScope.launch {
            _isLoading.value = true
            val existingTask = taskRepository.getTaskBySyncId(syncId)
            if (existingTask != null) {
                _task.value = existingTask
                _title.value = existingTask.title
                _description.value = existingTask.description
                _dueDate.value = existingTask.dueDate
                _isImportant.value = existingTask.isImportant
                _notes.value = existingTask.notes
            }
            _isLoading.value = false
        }
    }

    fun updateTitle(newTitle: String) {
        _title.value = newTitle
    }

    fun updateDescription(newDescription: String) {
        _description.value = newDescription
    }

    fun updateDueDate(newDate: Date) {
        _dueDate.value = newDate
    }

    fun updateIsImportant(value: Boolean) {
        _isImportant.value = value
    }

    fun updateNotes(newNotes: String) {
        _notes.value = newNotes
    }

    fun saveTask(onSuccess: () -> Unit) {
        viewModelScope.launch {
            val existingTask = _task.value
            val taskToSave = if (existingTask != null) {
                existingTask.apply {
                    title = _title.value
                    description = _description.value
                    dueDate = _dueDate.value
                    isImportant = _isImportant.value
                    notes = _notes.value
                }
            } else {
                Task(
                    syncId = UUID.randomUUID().toString(),
                    title = _title.value,
                    description = _description.value,
                    dueDate = _dueDate.value,
                    isImportant = _isImportant.value,
                    notes = _notes.value
                )
            }

            if (existingTask != null) {
                taskRepository.updateTask(taskToSave)
            } else {
                taskRepository.insertTask(taskToSave)
            }

            onSuccess()
        }
    }

    fun deleteTask(onSuccess: () -> Unit) {
        viewModelScope.launch {
            _task.value?.let { task ->
                taskRepository.deleteTask(task)
            }
            onSuccess()
        }
    }
}