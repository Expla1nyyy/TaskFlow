package com.expla1nyyy.taskflow.data.models

import androidx.room.Entity
import androidx.room.PrimaryKey
import com.google.gson.annotations.SerializedName
import java.util.Date

@Entity(tableName = "tasks")
data class Task(
    @PrimaryKey
    @SerializedName("sync_id")
    val syncId: String = "",

    @SerializedName("title")
    var title: String = "",

    @SerializedName("description")
    var description: String = "",

    @SerializedName("due_date")
    var dueDate: Date = Date(),

    @SerializedName("created_date")
    var createdDate: Date = Date(),

    @SerializedName("completion_date")
    var completionDate: Date? = null,

    @SerializedName("is_completed")
    var isCompleted: Boolean = false,

    @SerializedName("is_important")
    var isImportant: Boolean = false,

    @SerializedName("notes")
    var notes: String = ""
)