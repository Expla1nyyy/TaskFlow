package com.expla1nyyy.taskflow.presentation.screens.main

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.expla1nyyy.taskflow.data.models.Task
import com.expla1nyyy.taskflow.presentation.ui.theme.CompletedColor
import com.expla1nyyy.taskflow.presentation.ui.theme.ImportantColor
import com.expla1nyyy.taskflow.presentation.ui.theme.PrimaryColor
import java.text.SimpleDateFormat
import java.util.*

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainScreen(
    viewModel: MainViewModel,
    onNavigateToTaskEdit: (String) -> Unit,
    onLogout: () -> Unit
) {
    val activeTasks by viewModel.activeTasks.collectAsStateWithLifecycle()
    val completedTasks by viewModel.completedTasks.collectAsStateWithLifecycle()
    val isSyncing by viewModel.isSyncing.collectAsStateWithLifecycle()
    val error by viewModel.error.collectAsStateWithLifecycle()
    val username by viewModel.username.collectAsStateWithLifecycle()
    var showCompleted by remember { mutableStateOf(false) }
    val dateFormat = SimpleDateFormat("dd MMMM, HH:mm", Locale("ru"))

    LaunchedEffect(error) {
        if (error != null) {
            // Показать snackbar
        }
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Column {
                        Text("TaskFlow", fontSize = 20.sp, color = PrimaryColor)
                        if (username != null) {
                            Text(username!!, fontSize = 12.sp, color = Color.Gray)
                        }
                    }
                },
                actions = {
                    IconButton(onClick = { viewModel.syncWithServer() }) {
                        if (isSyncing) {
                            CircularProgressIndicator(modifier = Modifier.size(24.dp))
                        } else {
                            Icon(Icons.Default.Refresh, contentDescription = "Синхронизация")
                        }
                    }
                    IconButton(onClick = onLogout) {
                        Icon(Icons.Default.ExitToApp, contentDescription = "Выйти")
                    }
                },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = Color.Transparent)
            )
        },
        floatingActionButton = {
            FloatingActionButton(
                onClick = { onNavigateToTaskEdit("new") },
                containerColor = PrimaryColor
            ) {
                Icon(Icons.Default.Add, contentDescription = "Добавить задачу")
            }
        }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(16.dp)
        ) {
            // Active tasks
            Text(
                text = "Активные задачи (${activeTasks.size})",
                fontSize = 18.sp,
                fontWeight = androidx.compose.ui.text.font.FontWeight.Bold,
                modifier = Modifier.padding(bottom = 8.dp)
            )

            LazyColumn(
                modifier = Modifier.weight(if (showCompleted) 0.6f else 1f)
            ) {
                items(activeTasks) { task ->
                    TaskItem(
                        task = task,
                        dateFormat = dateFormat,
                        onTaskClick = { onNavigateToTaskEdit(task.syncId) },
                        onComplete = { viewModel.completeTask(task) },
                        onDelete = { viewModel.deleteTask(task) }
                    )
                }
            }

            // Completed tasks section
            Card(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(top = 8.dp)
                    .clickable { showCompleted = !showCompleted },
                colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surfaceVariant)
            ) {
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(16.dp),
                    horizontalArrangement = Arrangement.SpaceBetween
                ) {
                    Text("Выполненные задачи (${completedTasks.size})")
                    Icon(
                        if (showCompleted) Icons.Default.KeyboardArrowUp else Icons.Default.KeyboardArrowDown,
                        contentDescription = null
                    )
                }
            }

            if (showCompleted) {
                LazyColumn(modifier = Modifier.weight(0.4f)) {
                    items(completedTasks) { task ->
                        CompletedTaskItem(
                            task = task,
                            dateFormat = dateFormat,
                            onDelete = { viewModel.deleteTask(task) }
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun TaskItem(
    task: Task,
    dateFormat: SimpleDateFormat,
    onTaskClick: () -> Unit,
    onComplete: () -> Unit,
    onDelete: () -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(bottom = 8.dp)
            .clickable { onTaskClick() },
        colors = CardDefaults.cardColors(
            containerColor = if (task.isImportant) ImportantColor.copy(alpha = 0.1f) else MaterialTheme.colorScheme.surface
        ),
        border = if (task.isImportant) androidx.compose.foundation.BorderStroke(2.dp, ImportantColor) else null
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Column(modifier = Modifier.weight(1f)) {
                Text(
                    task.title,
                    fontSize = 16.sp,
                    fontWeight = androidx.compose.ui.text.font.FontWeight.SemiBold
                )
                Text(
                    "Выполнить до ${dateFormat.format(task.dueDate)}",
                    fontSize = 12.sp,
                    color = Color.Gray
                )
            }

            Row {
                if (task.isImportant) {
                    Text("★", color = ImportantColor, fontSize = 20.sp)
                    Spacer(modifier = Modifier.width(8.dp))
                }

                IconButton(onClick = onComplete) {
                    Icon(Icons.Default.Check, contentDescription = "Выполнить", tint = CompletedColor)
                }

                IconButton(onClick = onDelete) {
                    Icon(Icons.Default.Delete, contentDescription = "Удалить", tint = Color.Red)
                }
            }
        }
    }
}

@Composable
fun CompletedTaskItem(
    task: Task,
    dateFormat: SimpleDateFormat,
    onDelete: () -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(bottom = 8.dp),
        colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surfaceVariant)
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Column(modifier = Modifier.weight(1f)) {
                Text(
                    task.title,
                    fontSize = 14.sp,
                    color = Color.Gray
                )
                Text(
                    "Выполнено ${dateFormat.format(task.completionDate ?: Date())}",
                    fontSize = 11.sp,
                    color = Color.Gray
                )
            }

            IconButton(onClick = onDelete) {
                Icon(Icons.Default.Delete, contentDescription = "Удалить", tint = Color.Red)
            }
        }
    }
}