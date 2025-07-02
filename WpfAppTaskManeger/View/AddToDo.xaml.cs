using System.Windows;
using System.Windows.Input;
using WpfAppTaskManeger.Model;

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Логика взаимодействия для окна AddToDo.xaml, предназначенного для добавления новой задачи
    /// </summary>
    public partial class AddToDo : Window
    {
        // Значения по умолчанию
        public DateTime defDate = DateTime.Today;
        public string defDescription = "Описания нет";

        /// <summary>
        /// Пользовательская команда для сохранения новой задачи.
        /// Изменено на статическое свойство для лучшего разрешения в XAML.
        /// </summary>
        public readonly static RoutedCommand SaveNewToDoCommand= new RoutedCommand();

        /// <summary>
        /// Свойство для хранения новой созданной задачи.
        /// </summary>
        public TaskItem NewTask { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса AddToDo.
        /// Устанавливает значения по умолчанию для полей даты и описания.
        /// </summary>
        public AddToDo()
        {
            InitializeComponent();
            dateToDo.SelectedDate = defDate;
            descriptionToDo.Text = defDescription;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку "Сохранить дело".
        /// Вызывает метод для сохранения новой задачи.
        /// </summary>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveNewToDoItem();
        }

        /// <summary>
        /// Обработчик выполнения команды SaveNewToDoCommand (например, по нажатию Enter).
        /// Вызывает метод для сохранения новой задачи.
        /// </summary>
        private void SaveNewToDo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveNewToDoItem();
        }

        /// <summary>
        /// Сохраняет новую задачу в свойство NewTask, если введены все необходимые данные.
        /// Устанавливает DialogResult в true и закрывает окно.
        /// </summary>
        private void SaveNewToDoItem()
        {
            if (dateToDo.SelectedDate != null && !string.IsNullOrWhiteSpace(titleToDo.Text))
            {
                NewTask = new TaskItem(titleToDo.Text, dateToDo.SelectedDate.Value, descriptionToDo.Text);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните название и выберите дату.", "Ошибка добавления", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
