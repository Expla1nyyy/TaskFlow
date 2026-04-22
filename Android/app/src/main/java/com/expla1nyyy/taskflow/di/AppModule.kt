package com.expla1nyyy.taskflow.di

import android.content.Context
import com.expla1nyyy.taskflow.data.network.ApiService
import com.expla1nyyy.taskflow.data.network.RetrofitClient
import com.expla1nyyy.taskflow.data.repository.AuthRepository
import com.expla1nyyy.taskflow.data.repository.TaskRepository
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.android.qualifiers.ApplicationContext
import dagger.hilt.components.SingletonComponent
import javax.inject.Singleton

@Module
@InstallIn(SingletonComponent::class)
object AppModule {

    @Provides
    @Singleton
    fun provideApiService(): ApiService {
        return RetrofitClient.instance
    }

    @Provides
    @Singleton
    fun provideAuthRepository(
        @ApplicationContext context: Context,
        apiService: ApiService
    ): AuthRepository {
        return AuthRepository(context, apiService)
    }

    @Provides
    @Singleton
    fun provideTaskRepository(
        @ApplicationContext context: Context,
        apiService: ApiService
    ): TaskRepository {
        return TaskRepository(context, apiService)
    }
}