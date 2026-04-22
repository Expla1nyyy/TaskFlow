package com.expla1nyyy.taskflow.presentation.navigation

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import com.expla1nyyy.taskflow.presentation.screens.auth.AuthScreen
import com.expla1nyyy.taskflow.presentation.screens.auth.AuthViewModel
import com.expla1nyyy.taskflow.presentation.screens.main.MainScreen
import com.expla1nyyy.taskflow.presentation.screens.main.MainViewModel
import com.expla1nyyy.taskflow.presentation.screens.task.TaskEditScreen
import com.expla1nyyy.taskflow.presentation.screens.task.TaskEditViewModel

sealed class Screen(val route: String) {
    object Auth : Screen("auth")
    object Main : Screen("main")
    object TaskEdit : Screen("task_edit/{taskId}") {
        fun passArgs(taskId: String = "new") = "task_edit/$taskId"
    }
}

@Composable
fun NavGraph(
    authViewModel: AuthViewModel = hiltViewModel(),
    mainViewModel: MainViewModel = hiltViewModel()
) {
    val navController = rememberNavController()
    val isLoggedIn by authViewModel.isLoggedIn.collectAsState(initial = false)

    LaunchedEffect(isLoggedIn) {
        if (isLoggedIn) {
            navController.navigate(Screen.Main.route) {
                popUpTo(Screen.Auth.route) { inclusive = true }
            }
        } else {
            navController.navigate(Screen.Auth.route) {
                popUpTo(0)
            }
        }
    }

    NavHost(
        navController = navController,
        startDestination = Screen.Auth.route
    ) {
        composable(Screen.Auth.route) {
            AuthScreen(
                authViewModel = authViewModel,
                onLoginSuccess = {
                    navController.navigate(Screen.Main.route) {
                        popUpTo(Screen.Auth.route) { inclusive = true }
                    }
                }
            )
        }

        composable(Screen.Main.route) {
            MainScreen(
                viewModel = mainViewModel,
                onNavigateToTaskEdit = { taskId ->
                    navController.navigate(Screen.TaskEdit.passArgs(taskId))
                },
                onLogout = {
                    navController.navigate(Screen.Auth.route) {
                        popUpTo(0)
                    }
                }
            )
        }

        composable(
            route = Screen.TaskEdit.route,
            arguments = listOf(navArgument("taskId") { type = NavType.StringType })
        ) { backStackEntry ->
            val taskId = backStackEntry.arguments?.getString("taskId") ?: "new"
            val taskEditViewModel: TaskEditViewModel = hiltViewModel()

            LaunchedEffect(taskId) {
                if (taskId != "new") {
                    taskEditViewModel.loadTask(taskId)
                }
            }

            TaskEditScreen(
                viewModel = taskEditViewModel,
                onNavigateBack = {
                    navController.popBackStack()
                }
            )
        }
    }
}