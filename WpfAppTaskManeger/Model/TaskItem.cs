using System.ComponentModel;

namespace WpfAppTaskManeger.Model
{
    /// <summary>
    /// Отдельный элемент задачи в списке дел
    /// Содержит информацию о названии, дате выполнения, описании и статусе выполнения
    /// </summary>
    public class TaskItem : INotifyPropertyChanged
    {
        // Приватные поля для хранения данных задачи
        private string _title;
        private DateTime _dueDate;
        private string _description;
        private bool _isCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получает или устанавливает название задачи
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        /// <summary>
        /// Получает или устанавливает дату, к которой задача должна быть выполнена
        /// </summary>
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged(nameof(DueDate));
                }
            }
        }

        /// <summary>
        /// Получает или устанавливает подробное описание задачи
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        /// <summary>
        /// Получает или устанавливает статус выполнения задачи (true - выполнено, false - не выполнено)
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с значениями по умолчанию
        /// </summary>
        public TaskItem()
        {
            Title = "Title";
            DueDate = DateTime.Now;
            Description = "Description";
            IsCompleted = false;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с заданными названием, датой и описанием
        /// </summary>
        public TaskItem(string title, DateTime date, string description)
        {
            Title = title;
            DueDate = date;
            Description = description;
            IsCompleted = false;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с заданными названием, датой, описанием и статусом выполнения
        /// </summary>
        public TaskItem(string title, DateTime date, string description, bool doing)
            : this(title, date, description)
        {
            IsCompleted = doing;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
