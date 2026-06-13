using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.Json.Serialization;

namespace TaskFlow
{
    public class TaskModel : INotifyPropertyChanged
    {
        private string title;
        private string description;
        private DateTime dueDate;
        private DateTime createdDate;
        private DateTime completionDate;
        private bool isCompleted;
        private bool isImportant;
        private string notes;
        private ICommand completeCommand;
        private string syncId;
        private bool isSynced;

        public TaskModel()
        {
            syncId = Guid.NewGuid().ToString();
            createdDate = DateTime.Now;
            dueDate = DateTime.Now.AddDays(1).Date.AddHours(18);
            title = "";
            description = "";
            notes = "";
            isSynced = false;
        }

        [JsonPropertyName("title")]
        public string Title
        {
            get => title ?? "";
            set { title = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("description")]
        public string Description
        {
            get => description ?? "";
            set { description = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("due_date")]
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DueTime));
            }
        }

        public string DueTime => DueDate.ToString("HH:mm");

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate
        {
            get => createdDate;
            set
            {
                createdDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CreatedTime));
            }
        }

        public string CreatedTime => CreatedDate.ToString("HH:mm");

        [JsonPropertyName("completion_date")]
        public DateTime CompletionDate
        {
            get => completionDate;
            set { completionDate = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("is_completed")]
        public bool IsCompleted
        {
            get => isCompleted;
            set { isCompleted = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("is_important")]
        public bool IsImportant
        {
            get => isImportant;
            set { isImportant = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("notes")]
        public string Notes
        {
            get => notes ?? "";
            set { notes = value; OnPropertyChanged(); }
        }

        public ICommand CompleteCommand
        {
            get => completeCommand;
            set { completeCommand = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("sync_id")]
        public string SyncId
        {
            get
            {
                if (string.IsNullOrEmpty(syncId))
                {
                    syncId = Guid.NewGuid().ToString();
                }
                return syncId;
            }
            set { syncId = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("is_synced")]
        public bool IsSynced
        {
            get => isSynced;
            set { isSynced = value; OnPropertyChanged(); }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}