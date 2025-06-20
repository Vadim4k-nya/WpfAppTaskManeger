namespace WpfAppTaskManeger.Model
{
    /// <summary>
    /// Отдельный элемент задачи в списке дел
    /// Содержит информацию о названии, дате выполнения, описании и статусе выполнения
    /// </summary>
    public class TaskItem
    {
        // Приватные поля для хранения данных задачи
        private string _title;
        private DateTime _dueDate;
        private string _description;
        private bool _isCompleted;

        /// <summary>
        /// Получает или устанавливает название задачи
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Получает или устанавливает дату, к которой задача должна быть выполнена
        /// </summary>
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        /// <summary>
        /// Получает или устанавливает подробное описание задачи
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Получает или устанавливает статус выполнения задачи (true - выполнено, false - не выполнено)
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с значениями по умолчанию
        /// </summary>
        public TaskItem()
        {
            Title = "Title";
            DueDate = DateTime.Now;
            Description = "Description";
            IsCompleted = true;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с заданными названием, датой и описанием
        /// </summary>
        public TaskItem(string title, DateTime date, string description)
        {
            Title = title;
            DueDate = date;
            Description = description;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с заданными названием, датой, описанием и статусом выполнения
        /// </summary>
        public TaskItem(string title, DateTime date, string description, bool doing) 
            : this(title, date, description)
        {
            IsCompleted = doing;
        }
    }
}
