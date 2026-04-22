package com.expla1nyyy.taskflow.presentation.screens.auth

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.expla1nyyy.taskflow.presentation.ui.theme.PrimaryColor

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun AuthScreen(
    authViewModel: AuthViewModel,
    onLoginSuccess: () -> Unit
) {
    val uiState by authViewModel.uiState.collectAsStateWithLifecycle()
    var currentTab by remember { mutableIntStateOf(0) } // 0 - login, 1 - register, 2 - recover
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var regUsername by remember { mutableStateOf("") }
    var regEmail by remember { mutableStateOf("") }
    var regPassword by remember { mutableStateOf("") }
    var regConfirmPassword by remember { mutableStateOf("") }
    var recoveryWord by remember { mutableStateOf("") }
    var recoverUsername by remember { mutableStateOf("") }
    var recoverWord by remember { mutableStateOf("") }
    var recoverNewPassword by remember { mutableStateOf("") }
    var recoverConfirmPassword by remember { mutableStateOf("") }
    var regError by remember { mutableStateOf<String?>(null) }
    var recoverError by remember { mutableStateOf<String?>(null) }

    LaunchedEffect(uiState.isSuccess) {
        if (uiState.isSuccess) {
            onLoginSuccess()
            authViewModel.resetState()
        }
    }

    LaunchedEffect(uiState.isRegisterSuccess) {
        if (uiState.isRegisterSuccess) {
            currentTab = 0
            authViewModel.resetState()
        }
    }

    LaunchedEffect(uiState.isRecoverSuccess) {
        if (uiState.isRecoverSuccess) {
            currentTab = 0
            authViewModel.resetState()
        }
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("TaskFlow", fontSize = 24.sp, color = PrimaryColor) },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = Color.Transparent)
            )
        }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(24.dp)
                .verticalScroll(rememberScrollState()),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // Tabs
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceEvenly
            ) {
                Button(
                    onClick = { currentTab = 0 },
                    colors = ButtonDefaults.buttonColors(
                        containerColor = if (currentTab == 0) PrimaryColor else Color.Transparent,
                        contentColor = if (currentTab == 0) Color.White else Color.Gray
                    ),
                    shape = MaterialTheme.shapes.small
                ) {
                    Text("Вход")
                }

                Button(
                    onClick = { currentTab = 1 },
                    colors = ButtonDefaults.buttonColors(
                        containerColor = if (currentTab == 1) PrimaryColor else Color.Transparent,
                        contentColor = if (currentTab == 1) Color.White else Color.Gray
                    ),
                    shape = MaterialTheme.shapes.small
                ) {
                    Text("Регистрация")
                }

                Button(
                    onClick = { currentTab = 2 },
                    colors = ButtonDefaults.buttonColors(
                        containerColor = if (currentTab == 2) PrimaryColor else Color.Transparent,
                        contentColor = if (currentTab == 2) Color.White else Color.Gray
                    ),
                    shape = MaterialTheme.shapes.small
                ) {
                    Text("Восстановление")
                }
            }

            Spacer(modifier = Modifier.height(24.dp))

            when (currentTab) {
                0 -> {
                    // Login Form
                    OutlinedTextField(
                        value = username,
                        onValueChange = { username = it },
                        label = { Text("Имя пользователя") },
                        modifier = Modifier.fillMaxWidth(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = password,
                        onValueChange = { password = it },
                        label = { Text("Пароль") },
                        modifier = Modifier.fillMaxWidth(),
                        visualTransformation = PasswordVisualTransformation(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(24.dp))

                    Button(
                        onClick = { authViewModel.login(username, password) },
                        modifier = Modifier.fillMaxWidth(),
                        enabled = !uiState.isLoading
                    ) {
                        Text("Войти")
                    }
                }

                1 -> {
                    // Register Form
                    OutlinedTextField(
                        value = regUsername,
                        onValueChange = { regUsername = it },
                        label = { Text("Имя пользователя") },
                        modifier = Modifier.fillMaxWidth(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = regEmail,
                        onValueChange = { regEmail = it },
                        label = { Text("Email") },
                        modifier = Modifier.fillMaxWidth(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = regPassword,
                        onValueChange = { regPassword = it },
                        label = { Text("Пароль") },
                        modifier = Modifier.fillMaxWidth(),
                        visualTransformation = PasswordVisualTransformation(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = regConfirmPassword,
                        onValueChange = { regConfirmPassword = it },
                        label = { Text("Подтверждение пароля") },
                        modifier = Modifier.fillMaxWidth(),
                        visualTransformation = PasswordVisualTransformation(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = recoveryWord,
                        onValueChange = { recoveryWord = it },
                        label = { Text("Кодовое слово") },
                        modifier = Modifier.fillMaxWidth(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(24.dp))

                    Button(
                        onClick = {
                            when {
                                regUsername.isBlank() -> regError = "Введите имя пользователя"
                                regEmail.isBlank() -> regError = "Введите email"
                                regPassword.isBlank() -> regError = "Введите пароль"
                                regPassword != regConfirmPassword -> regError = "Пароли не совпадают"
                                recoveryWord.isBlank() -> regError = "Введите кодовое слово"
                                else -> {
                                    regError = null
                                    authViewModel.register(regUsername, regEmail, regPassword, recoveryWord)
                                }
                            }
                        },
                        modifier = Modifier.fillMaxWidth(),
                        enabled = !uiState.isLoading
                    ) {
                        Text("Зарегистрироваться")
                    }

                    regError?.let {
                        Spacer(modifier = Modifier.height(16.dp))
                        Text(it, color = MaterialTheme.colorScheme.error)
                    }
                }

                2 -> {
                    // Recover Form
                    OutlinedTextField(
                        value = recoverUsername,
                        onValueChange = { recoverUsername = it },
                        label = { Text("Имя пользователя") },
                        modifier = Modifier.fillMaxWidth(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = recoverWord,
                        onValueChange = { recoverWord = it },
                        label = { Text("Кодовое слово") },
                        modifier = Modifier.fillMaxWidth(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = recoverNewPassword,
                        onValueChange = { recoverNewPassword = it },
                        label = { Text("Новый пароль") },
                        modifier = Modifier.fillMaxWidth(),
                        visualTransformation = PasswordVisualTransformation(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(16.dp))

                    OutlinedTextField(
                        value = recoverConfirmPassword,
                        onValueChange = { recoverConfirmPassword = it },
                        label = { Text("Подтверждение пароля") },
                        modifier = Modifier.fillMaxWidth(),
                        visualTransformation = PasswordVisualTransformation(),
                        singleLine = true
                    )

                    Spacer(modifier = Modifier.height(24.dp))

                    Button(
                        onClick = {
                            when {
                                recoverUsername.isBlank() -> recoverError = "Введите имя пользователя"
                                recoverWord.isBlank() -> recoverError = "Введите кодовое слово"
                                recoverNewPassword.isBlank() -> recoverError = "Введите новый пароль"
                                recoverNewPassword != recoverConfirmPassword -> recoverError = "Пароли не совпадают"
                                else -> {
                                    recoverError = null
                                    authViewModel.recoverPassword(recoverUsername, recoverWord, recoverNewPassword)
                                }
                            }
                        },
                        modifier = Modifier.fillMaxWidth(),
                        enabled = !uiState.isLoading
                    ) {
                        Text("Восстановить пароль")
                    }

                    recoverError?.let {
                        Spacer(modifier = Modifier.height(16.dp))
                        Text(it, color = MaterialTheme.colorScheme.error)
                    }
                }
            }

            if (uiState.isLoading) {
                Spacer(modifier = Modifier.height(16.dp))
                CircularProgressIndicator()
            }

            uiState.error?.let { error ->
                Spacer(modifier = Modifier.height(16.dp))
                Text(error, color = MaterialTheme.colorScheme.error)
            }

            if (uiState.isRegisterSuccess) {
                Spacer(modifier = Modifier.height(16.dp))
                Text("Регистрация успешна! Теперь войдите.", color = Color.Green)
            }

            if (uiState.isRecoverSuccess) {
                Spacer(modifier = Modifier.height(16.dp))
                Text("Пароль изменен! Теперь войдите.", color = Color.Green)
            }
        }
    }
}