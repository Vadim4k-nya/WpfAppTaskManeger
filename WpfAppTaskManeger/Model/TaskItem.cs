using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private DateTime _date;
        private string _description;
        private bool _doing;

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
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
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
        public bool Doing
        {
            get { return _doing; }
            set { _doing = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с значениями по умолчанию
        /// </summary>
        public TaskItem()
        {
            Title = "Title";
            Date = DateTime.Now;
            Description = "Description";
            Doing = true;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с заданными названием, датой и описанием
        /// </summary>
        public TaskItem(string title, DateTime date, string description)
        {
            Title = title;
            Date = date;
            Description = description;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса TaskItem с заданными названием, датой, описанием и статусом выполнения
        /// </summary>
        public TaskItem(string title, DateTime date, string description, bool doing) 
            : this(title, date, description)
        {
            Doing = doing;
        }
    }
}
