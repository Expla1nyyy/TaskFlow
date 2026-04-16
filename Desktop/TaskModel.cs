using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

        public string Title
        {
            get => title ?? "";
            set { title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => description ?? "";
            set { description = value; OnPropertyChanged(); }
        }

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

        public string DueTime => DueDate.ToString("HH:mm", CultureInfo.InvariantCulture);

        public DateTime CreatedDate
        {
            get => createdDate;
            set { createdDate = value; OnPropertyChanged(); OnPropertyChanged(nameof(CreatedTime)); }
        }

        public string CreatedTime => CreatedDate.ToString("HH:mm", CultureInfo.InvariantCulture);

        public DateTime CompletionDate
        {
            get => completionDate;
            set { completionDate = value; OnPropertyChanged(); OnPropertyChanged(nameof(CompletionTime)); }
        }

        public string CompletionTime => CompletionDate.ToString("HH:mm", CultureInfo.InvariantCulture);

        public bool IsCompleted
        {
            get => isCompleted;
            set { isCompleted = value; OnPropertyChanged(); }
        }

        public bool IsImportant
        {
            get => isImportant;
            set { isImportant = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => notes ?? "";
            set { notes = value; OnPropertyChanged(); }
        }

        public string SyncId
        {
            get => syncId ?? Guid.NewGuid().ToString();
            set { syncId = value; OnPropertyChanged(); }
        }

        public bool IsSynced
        {
            get => isSynced;
            set { isSynced = value; OnPropertyChanged(); }
        }

        public ICommand CompleteCommand
        {
            get
            {
                return completeCommand ??= new RelayCommand(
                    param => CompleteTask(),
                    param => !IsCompleted);
            }
        }

        private void CompleteTask()
        {
            IsCompleted = true;
            CompletionDate = DateTime.Now;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}