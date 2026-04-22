package com.expla1nyyy.taskflow.presentation.screens.task

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.expla1nyyy.taskflow.presentation.ui.theme.PrimaryColor
import java.text.SimpleDateFormat
import java.util.*

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun TaskEditScreen(
    viewModel: TaskEditViewModel,
    onNavigateBack: () -> Unit
) {
    val title by viewModel.title.collectAsStateWithLifecycle()
    val description by viewModel.description.collectAsStateWithLifecycle()
    val dueDate by viewModel.dueDate.collectAsStateWithLifecycle()
    val isImportant by viewModel.isImportant.collectAsStateWithLifecycle()
    val notes by viewModel.notes.collectAsStateWithLifecycle()
    val isLoading by viewModel.isLoading.collectAsStateWithLifecycle()
    val existingTask by viewModel.task.collectAsStateWithLifecycle()

    var showDatePicker by remember { mutableStateOf(false) }
    var showTimePicker by remember { mutableStateOf(false) }
    val dateFormat = SimpleDateFormat("dd MMMM yyyy", Locale("ru"))
    val timeFormat = SimpleDateFormat("HH:mm", Locale("ru"))

    // Временные переменные для выбора даты и времени
    var tempYear by remember { mutableIntStateOf(dueDate.year + 1900) }
    var tempMonth by remember { mutableIntStateOf(dueDate.month) }
    var tempDay by remember { mutableIntStateOf(dueDate.date) }
    var tempHour by remember { mutableIntStateOf(dueDate.hours) }
    var tempMinute by remember { mutableIntStateOf(dueDate.minutes) }

    // Обновляем временные переменные при изменении dueDate
    LaunchedEffect(dueDate) {
        tempYear = dueDate.year + 1900
        tempMonth = dueDate.month
        tempDay = dueDate.date
        tempHour = dueDate.hours
        tempMinute = dueDate.minutes
    }

    if (showDatePicker) {
        AlertDialog(
            onDismissRequest = { showDatePicker = false },
            confirmButton = {
                TextButton(
                    onClick = {
                        val calendar = Calendar.getInstance()
                        calendar.set(tempYear, tempMonth, tempDay, dueDate.hours, dueDate.minutes)
                        viewModel.updateDueDate(calendar.time)
                        showDatePicker = false
                    }
                ) {
                    Text("OK")
                }
            },
            dismissButton = {
                TextButton(onClick = { showDatePicker = false }) {
                    Text("Отмена")
                }
            },
            title = { Text("Выберите дату") },
            text = {
                DatePicker(
                    year = tempYear,
                    month = tempMonth,
                    day = tempDay,
                    onDateChange = { year, month, day ->
                        tempYear = year
                        tempMonth = month
                        tempDay = day
                    }
                )
            }
        )
    }

    if (showTimePicker) {
        AlertDialog(
            onDismissRequest = { showTimePicker = false },
            confirmButton = {
                TextButton(
                    onClick = {
                        val calendar = Calendar.getInstance()
                        calendar.set(dueDate.year + 1900, dueDate.month, dueDate.date, tempHour, tempMinute)
                        viewModel.updateDueDate(calendar.time)
                        showTimePicker = false
                    }
                ) {
                    Text("OK")
                }
            },
            dismissButton = {
                TextButton(onClick = { showTimePicker = false }) {
                    Text("Отмена")
                }
            },
            title = { Text("Выберите время") },
            text = {
                TimePicker(
                    hour = tempHour,
                    minute = tempMinute,
                    onTimeChange = { hour, minute ->
                        tempHour = hour
                        tempMinute = minute
                    }
                )
            }
        )
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text(if (existingTask == null) "Новая задача" else "Редактирование") },
                navigationIcon = {
                    IconButton(onClick = onNavigateBack) {
                        Icon(Icons.Default.ArrowBack, contentDescription = "Назад")
                    }
                },
                actions = {
                    if (existingTask != null) {
                        IconButton(onClick = { viewModel.deleteTask(onNavigateBack) }) {
                            Icon(Icons.Default.Delete, contentDescription = "Удалить")
                        }
                    }
                },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = PrimaryColor)
            )
        }
    ) { paddingValues ->
        if (isLoading) {
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(paddingValues),
                contentAlignment = Alignment.Center
            ) {
                CircularProgressIndicator()
            }
        } else {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(paddingValues)
                    .padding(16.dp)
                    .verticalScroll(rememberScrollState())
            ) {
                OutlinedTextField(
                    value = title,
                    onValueChange = { viewModel.updateTitle(it) },
                    label = { Text("Название задачи *") },
                    modifier = Modifier.fillMaxWidth(),
                    singleLine = true
                )

                Spacer(modifier = Modifier.height(16.dp))

                OutlinedTextField(
                    value = description,
                    onValueChange = { viewModel.updateDescription(it) },
                    label = { Text("Описание") },
                    modifier = Modifier.fillMaxWidth(),
                    minLines = 3
                )

                Spacer(modifier = Modifier.height(16.dp))

                Text("Срок выполнения", style = MaterialTheme.typography.labelMedium)
                Spacer(modifier = Modifier.height(8.dp))

                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    OutlinedButton(
                        onClick = { showDatePicker = true },
                        modifier = Modifier.weight(1f)
                    ) {
                        Text(dateFormat.format(dueDate))
                    }

                    OutlinedButton(
                        onClick = { showTimePicker = true },
                        modifier = Modifier.weight(1f)
                    ) {
                        Text(timeFormat.format(dueDate))
                    }
                }

                Spacer(modifier = Modifier.height(16.dp))

                Row(
                    modifier = Modifier.fillMaxWidth(),
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text("Важная задача", modifier = Modifier.weight(1f))
                    Switch(
                        checked = isImportant,
                        onCheckedChange = { viewModel.updateIsImportant(it) }
                    )
                }

                Spacer(modifier = Modifier.height(16.dp))

                OutlinedTextField(
                    value = notes,
                    onValueChange = { viewModel.updateNotes(it) },
                    label = { Text("Заметки") },
                    modifier = Modifier.fillMaxWidth(),
                    minLines = 5
                )

                Spacer(modifier = Modifier.height(24.dp))

                Button(
                    onClick = { viewModel.saveTask(onNavigateBack) },
                    modifier = Modifier.fillMaxWidth(),
                    enabled = title.isNotBlank()
                ) {
                    Text("Сохранить")
                }
            }
        }
    }
}

@Composable
fun DatePicker(
    year: Int,
    month: Int,
    day: Int,
    onDateChange: (year: Int, month: Int, day: Int) -> Unit
) {
    Column {
        Text("Год: $year")
        Slider(
            value = year.toFloat(),
            onValueChange = { onDateChange(it.toInt(), month, day) },
            valueRange = 2020f..2030f,
            steps = 10
        )
        Text("Месяц: ${month + 1}")
        Slider(
            value = month.toFloat(),
            onValueChange = { onDateChange(year, it.toInt(), day) },
            valueRange = 0f..11f,
            steps = 11
        )
        Text("День: $day")
        Slider(
            value = day.toFloat(),
            onValueChange = { onDateChange(year, month, it.toInt()) },
            valueRange = 1f..31f,
            steps = 30
        )
    }
}

@Composable
fun TimePicker(
    hour: Int,
    minute: Int,
    onTimeChange: (hour: Int, minute: Int) -> Unit
) {
    Column {
        Text("Час: $hour")
        Slider(
            value = hour.toFloat(),
            onValueChange = { onTimeChange(it.toInt(), minute) },
            valueRange = 0f..23f,
            steps = 23
        )
        Text("Минута: $minute")
        Slider(
            value = minute.toFloat(),
            onValueChange = { onTimeChange(hour, it.toInt()) },
            valueRange = 0f..59f,
            steps = 59
        )
    }
}