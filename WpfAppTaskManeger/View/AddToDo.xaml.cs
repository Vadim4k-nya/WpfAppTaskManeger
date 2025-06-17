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
        // Значение даты по умолчанию для поля ввода даты
        public DateTime defDate = DateTime.Today;
        // Значение описания по умолчанию для поля ввода описания
        public string defDescription = "Описания нет";

        // Пользовательская команда для сохранения новой задачи
        public static readonly RoutedCommand SaveNewToDoCommand = new RoutedCommand();

        /// <summary>
        /// Инициализирует новый экземпляр класса AddToDo
        /// Устанавливает значения по умолчанию для полей даты и описания
        /// </summary>
        public AddToDo()
        {
            InitializeComponent();
            dateToDo.SelectedDate = defDate;
            descriptionToDo.Text = defDescription;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку "Сохранить дело"
        /// Вызывает метод для сохранения новой задачи
        /// </summary>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveNewToDoItem();
        }

        /// <summary>
        /// Обработчик выполнения команды SaveNewToDoCommand (например, по нажатию Enter)
        /// Вызывает метод для сохранения новой задачи
        /// </summary>
        private void SaveNewToDo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveNewToDoItem();
        }

        /// <summary>
        /// Сохраняет новую задачу в список дел, если введены все необходимые данные
        /// Очищает поля ввода после успешного сохранения и закрывает окно
        /// </summary>
        private void SaveNewToDoItem()
        {
            if (dateToDo.SelectedDate != null && !string.IsNullOrWhiteSpace(titleToDo.Text))
            {
                // Добавление новой задачи в статический список MainWindow
                MainWindow.toDoList.Add(new TaskItem(titleToDo.Text, dateToDo.SelectedDate.Value, descriptionToDo.Text));

                this.Close();
            }
            else
            {
                // Вывод предупреждения, если обязательные поля не заполнены
                MessageBox.Show("Пожалуйста, заполните название и выберите дату.", "Ошибка добавления", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
