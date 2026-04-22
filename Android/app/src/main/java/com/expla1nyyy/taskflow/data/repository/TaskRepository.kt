package com.expla1nyyy.taskflow.data.repository

import android.content.Context
import com.expla1nyyy.taskflow.data.database.TaskDatabase
import com.expla1nyyy.taskflow.data.models.SyncRequest
import com.expla1nyyy.taskflow.data.models.Task
import com.expla1nyyy.taskflow.data.network.ApiService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.first
import kotlinx.coroutines.withContext
import javax.inject.Inject
import javax.inject.Singleton

@Singleton
class TaskRepository @Inject constructor(
    private val context: Context,
    private val apiService: ApiService
) {

    private val taskDao = TaskDatabase.getDatabase(context).taskDao()

    fun getActiveTasks(): Flow<List<Task>> = taskDao.getActiveTasks()

    fun getCompletedTasks(): Flow<List<Task>> = taskDao.getCompletedTasks()

    suspend fun getTaskBySyncId(syncId: String): Task? = taskDao.getTaskBySyncId(syncId)

    suspend fun insertTask(task: Task) = taskDao.insertTask(task)

    suspend fun updateTask(task: Task) = taskDao.updateTask(task)

    suspend fun deleteTask(task: Task) = taskDao.deleteTask(task)

    suspend fun deleteAllTasks() = taskDao.deleteAllTasks()

    suspend fun syncWithServer(token: String): Boolean {
        return withContext(Dispatchers.IO) {
            try {
                val activeTasksList = taskDao.getActiveTasks().first()
                val completedTasksList = taskDao.getCompletedTasks().first()
                val allTasks = activeTasksList + completedTasksList

                val syncRequest = SyncRequest(tasks = allTasks, last_sync = null)
                val response = apiService.syncTasks("Bearer $token", syncRequest)

                taskDao.deleteAllTasks()
                response.tasks.forEach { task ->
                    taskDao.insertTask(task)
                }

                true
            } catch (e: Exception) {
                e.printStackTrace()
                false
            }
        }
    }
}