using System;
using System.ComponentModel;

namespace TaskFlow
{
    public class TaskModel : INotifyPropertyChanged
    {
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private bool _isCompleted;
        private bool _isImportant;
        private DateTime _createdDate = DateTime.Now;
        private string _notes;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        public string DueTime => DueDate.ToString("HH:mm");

        public string DueDateFormatted => DueDate.ToString("dd.MM.yyyy");

        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }

        public bool IsImportant
        {
            get => _isImportant;
            set { _isImportant = value; OnPropertyChanged(nameof(IsImportant)); }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set { _createdDate = value; OnPropertyChanged(nameof(CreatedDate)); }
        }

        public string CreatedTime => CreatedDate.ToString("HH:mm");

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(nameof(Notes)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}