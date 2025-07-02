using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppTaskManeger.Model;
using WpfAppTaskManeger.ViewModel;

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// Главное окно приложения, отображающее список дел
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        /// <summary>
        /// Инициализирует новый экземпляр класса MainWindow.
        /// Устанавливает DataContext окна на экземпляр MainViewModel.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события загрузки главного окна.
        /// Вызывает методы ViewModel для инициализации данных.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.EndToDo();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Добавить" в главном окне.
        /// Открывает новое окно для добавления задачи.
        /// </summary>
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenAddToDoWindow();
        }

        /// <summary>
        /// Открывает дочернее окно для добавления новой задачи.
        /// Обновляет список и сохраняет данные после закрытия дочернего окна.
        /// </summary>
        private void OpenAddToDoWindow()
        {
            AddToDo addToDoWindow = new AddToDo();
            addToDoWindow.Owner = this;

            if (addToDoWindow.ShowDialog() == true)
            {
                if (addToDoWindow.NewTask != null)
                {
                    _viewModel.ToDoList.Add(addToDoWindow.NewTask);
                }
            }
            _viewModel.EndToDo();
        }

        /// <summary>
        /// Обработчик выполнения команды ApplicationCommands.New (например, Ctrl+N).
        /// Открывает окно для добавления новой задачи.
        /// </summary>
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenAddToDoWindow();
        }

        /// <summary>
        /// Обработчик выполнения команды ApplicationCommands.Save (например, Ctrl+S).
        /// Вызывает команду ViewModel для сохранения списка дел в текстовый файл.
        /// </summary>
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.SaveTasksTxtCommand.Execute(null);
        }

        /// <summary>
        /// Обработчик выполнения команды ApplicationCommands.Delete.
        /// Вызывает команду ViewModel для удаления выбранной задачи.
        /// </summary>
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.DeleteTaskCommand.Execute(null);
        }

        /// <summary>
        /// Обработчик CanExecute для команды ApplicationCommands.Delete.
        /// Определяет, может ли команда быть выполнена, основываясь на состоянии ViewModel.
        /// </summary>
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewModel.CanDeleteSelectedTask(_viewModel.SelectedTask);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Удалить" внутри элемента списка.
        /// Удаляет выбранную задачу из списка после подтверждения.
        /// </summary>
        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button?.DataContext is TaskItem taskToDelete)
            {
                _viewModel.SelectedTask = taskToDelete;
                _viewModel.DeleteTaskCommand.Execute(null);
            }
        }

        /// <summary>
        /// Обработчик события установки флажка (CheckBox) для задачи.
        /// Вызывает команду ViewModel для переключения статуса выполнения задачи.
        /// </summary>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                _viewModel.ToggleTaskCompletionCommand.Execute(task);
            }
        }

        /// <summary>
        /// Обработчик события снятия флажка (CheckBox) для задачи.
        /// Вызывает команду ViewModel для переключения статуса выполнения задачи.
        /// </summary>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                _viewModel.ToggleTaskCompletionCommand.Execute(task);
            }
        }
    }
}
