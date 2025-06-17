using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppTaskManeger.Model;

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// Главное окно приложения, отображающее список дел
    /// </summary>
    public partial class MainWindow : Window
    {
        // Статический список дел, доступный из других частей приложения
        public static List<TaskItem> toDoList = new List<TaskItem>();

        // Путь к папке для хранения файлов данных
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        // Полный путь к JSON-файлу с данными задач
        private readonly string _filePath;

        // Пользовательская команда для удаления выбранной задачи
        public static readonly RoutedCommand DeleteToDoCommand = new RoutedCommand();

        /// <summary>
        /// Инициализирует новый экземпляр класса MainWindow
        /// Устанавливает путь к файлу данных и инициализирует список тестовыми данными, если список пуст
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _filePath = Path.Combine(_folderPath, "todo.json");

            // Инициализация тестовых данных, если список пуст
            // Эти данные будут загружены только при первом запуске, если нет сохраненного файла
            if (toDoList.Count == 0)
            {
                toDoList.Add(new TaskItem("Приготовить покушать", new(2024, 01, 15), "Нет описания"));
                toDoList.Add(new TaskItem("Поработать", new(2024, 01, 20), "Съездить на совещание в Москву"));
                toDoList.Add(new TaskItem("Отдохнуть", new(2024, 01, 02), "Съездить в отпуск в Сочи"));
            }

            // Установка контекста данных для ListBox
            listToDo.ItemsSource = toDoList;
            listToDo.Items.Refresh();

            UpdateProgress();
        }

        /// <summary>
        /// Обработчик события загрузки главного окна
        /// Загружает данные из JSON-файла и обновляет прогресс-бар
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadJsonFile();
            EndToDo();
        }

        /// <summary>
        /// Обработчик события закрытия главного окна
        /// Сохраняет данные в JSON-файл
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            SaveJsonFile();
        }

        /// <summary>
        /// Обновляет значения прогресс-бара и текстового отображения выполненных задач
        /// </summary>
        private void UpdateProgress()
        {
            int completedTasks = toDoList.Count(t => t.Doing); // Количество выполненных задач
            int totalTasks = toDoList.Count;                   // Общее количество задач

            // Обновление значения ProgressBar
            progressToDo.Maximum = totalTasks;
            progressToDo.Value = completedTasks;

            // Обновление текстового отображения прогресса
            progressTextToDo.Text = $"{completedTasks}/{totalTasks}";
        }

        /// <summary>
        /// Сохраняет список задач в JSON-файл
        /// Создает директорию, если она не существует
        /// </summary>
        private void SaveJsonFile()
        {
            try
            {
                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                }
                string json = JsonConvert.SerializeObject(toDoList, Formatting.Indented);

                using (StreamWriter sw = new StreamWriter(_filePath))
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении JSON файла: {ex.Message}", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загружает список задач из JSON-файла
        /// Если файл не существует или произошла ошибка, список остается пустым или тестовым
        /// </summary>
        private void LoadJsonFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);

                    var loadedToDos = JsonConvert.DeserializeObject<List<TaskItem>>(json);

                    toDoList.Clear();
                    if (loadedToDos != null)
                    {
                        foreach (var item in loadedToDos)
                        {
                            toDoList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке JSON файла: {ex.Message}", "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                listToDo.Items.Refresh();
                UpdateProgress();
            }
        }

        /// <summary>
        /// Сохраняет список задач в текстовый файл
        /// Открывает диалоговое окно для выбора места сохранения
        /// </summary>
        private void SaveTxtFile()
        {
            if (toDoList.Count() == 0)
            {
                MessageBox.Show("В списке нет дел.", "Список пуст", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Сохранить список дел";
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.FileName = "СписокДел.txt";
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Список дел:");
                sb.AppendLine("--------------------------------------------------");

                foreach (var todoItem in toDoList)
                {
                    sb.AppendLine($"{(todoItem.Doing ? "✔" : "")}{todoItem.Title}");
                    sb.AppendLine();
                    sb.AppendLine($"{todoItem.Description}");
                    sb.AppendLine();
                    sb.AppendLine($"{todoItem.Date:dd.MM.yyyy}");
                    sb.AppendLine();
                    sb.AppendLine();
                }
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());
            }
        }

        /// <summary>
        /// Метод вызывается при изменении списка дел (добавление, удаление, изменение статуса)
        /// Обновляет отображение списка и прогресс-бар, а также сохраняет данные
        /// Это централизованная точка для обновления UI и сохранения состояния
        /// </summary>
        public void EndToDo()
        {
            listToDo.Items.Refresh(); // Обновление привязки данных ListBox
            UpdateProgress();         // Обновление прогресс-бара
            SaveJsonFile();           // Сохранение изменений в файл
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Сохранить" в главном окне
        /// Вызывает метод для сохранения списка дел в текстовый файл
        /// </summary>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTxtFile();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Добавить" в главном окне
        /// Открывает новое окно для добавления задачи
        /// </summary>
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenAddToDoWindow();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Удалить" внутри элемента списка
        /// Удаляет выбранную задачу из списка после подтверждения
        /// </summary>
        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            // Получаем DataContext кнопки, который является TaskItem, связанный с этой строкой списка.
            Button? button = sender as Button;
            TaskItem? taskToDelete = button?.DataContext as TaskItem;

            if (taskToDelete != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить дело?",
                    "Удаление дела",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    toDoList.Remove(taskToDelete);
                    EndToDo();
                }
            }
            else
            {
                // Это сообщение, вероятно, не будет показано, так как кнопка всегда привязана к TaskItem
                // но оставлено для полноты обработки возможных сценариев
                MessageBox.Show("Не удалось определить дело для удаления. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик события установки флажка (CheckBox) для задачи
        /// Изменяет статус выполнения задачи на "выполнено" и обновляет UI
        /// </summary>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Получаем DataContext CheckBox, который является TaskItem, связанный с этой строкой списка
            CheckBox? checkBox = sender as CheckBox;
            TaskItem? changedTask = checkBox?.DataContext as TaskItem;

            if (changedTask != null)
            {
                changedTask.Doing = true;
                EndToDo();
            }
        }

        /// <summary>
        /// Обработчик события снятия флажка (CheckBox) для задачи
        /// Изменяет статус выполнения задачи на "не выполнено" и обновляет UI
        /// </summary>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Получаем DataContext CheckBox, который является TaskItem, связанный с этой строкой списка
            CheckBox? checkBox = sender as CheckBox;
            TaskItem? changedTask = checkBox?.DataContext as TaskItem;

            if (changedTask != null)
            {
                changedTask.Doing = false;
                EndToDo();
            }
        }

        /// <summary>
        /// Обработчик выполнения команды ApplicationCommands.New (например, Ctrl+N)
        /// Открывает окно для добавления новой задачи
        /// </summary>
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenAddToDoWindow();
        }

        /// <summary>
        /// Открывает дочернее окно для добавления новой задачи
        /// Обновляет список и сохраняет данные после закрытия дочернего окна
        /// </summary>
        private void OpenAddToDoWindow()
        {
            AddToDo addToDoWindow = new AddToDo();
            addToDoWindow.Owner = this; // Устанавливаем владельца окна, чтобы оно было центрировано относительно родителя
            addToDoWindow.ShowDialog(); // Открываем окно как модальное диалоговое, блокируя родительское

            EndToDo();
        }

        /// <summary>
        /// Обработчик выполнения команды ApplicationCommands.Save (например, Ctrl+S)
        /// Вызывает метод для сохранения списка дел в текстовый файл
        /// </summary>
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveTxtFile();
        }

        /// <summary>
        /// Обработчик выполнения команды DeleteToDoCommand
        /// Удаляет выбранную задачу из списка после подтверждения
        /// Эта команда обрабатывает удаление через выбор элемента в списке
        /// </summary>
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TaskItem? taskToDelete = listToDo.SelectedItem as TaskItem;
            if (taskToDelete != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить дело?",
                    "Удаление дела",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    toDoList.Remove(taskToDelete);
                    EndToDo();
                }
            }
            else
            {
                // Вывод предупреждения, если для удаления не выбран элемент.
                MessageBox.Show("Пожалуйста, выберите дело для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}