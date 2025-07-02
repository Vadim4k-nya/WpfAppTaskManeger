using Microsoft.Win32; 
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfAppTaskManeger.Model;

namespace WpfAppTaskManeger.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TaskItem> _toDoList;
        private TaskItem _selectedTask;
        private int _completedTasks;
        private int _totalTasks;

        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        private readonly string _filePath;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Коллекция задач для отображения в UI. ObservableCollection автоматически уведомляет UI об изменениях.
        /// </summary>
        public ObservableCollection<TaskItem> ToDoList
        {
            get { return _toDoList; }
            set
            {
                if (_toDoList != value)
                {
                    _toDoList = value;
                    OnPropertyChanged(nameof(ToDoList));
                }
            }
        }

        /// <summary>
        /// Выбранная задача в списке. Используется для операций, таких как удаление.
        /// </summary>
        public TaskItem SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                }
            }
        }

        /// <summary>
        /// Количество выполненных задач.
        /// </summary>
        public int CompletedTasks
        {
            get { return _completedTasks; }
            set
            {
                if (_completedTasks != value)
                {
                    _completedTasks = value;
                    OnPropertyChanged(nameof(CompletedTasks));
                    OnPropertyChanged(nameof(ProgressTextToDo));
                }
            }
        }

        /// <summary>
        /// Общее количество задач.
        /// </summary>
        public int TotalTasks
        {
            get { return _totalTasks; }
            set
            {
                if (_totalTasks != value)
                {
                    _totalTasks = value;
                    OnPropertyChanged(nameof(TotalTasks));
                    OnPropertyChanged(nameof(ProgressTextToDo));
                }
            }
        }

        /// <summary>
        /// Текстовое представление прогресса.
        /// </summary>
        public string ProgressTextToDo
        {
            get { return $"{CompletedTasks}/{TotalTasks}"; }
        }

        public ICommand DeleteTaskCommand { get; private set; }
        public ICommand SaveTasksTxtCommand { get; private set; }
        public ICommand ToggleTaskCompletionCommand { get; private set; }

        /// <summary>
        /// Конструктор MainViewModel. Инициализирует коллекцию задач, загружает данные и настраивает команды.
        /// </summary>
        public MainViewModel()
        {
            _filePath = Path.Combine(_folderPath, "todo.json");
            ToDoList = new ObservableCollection<TaskItem>();

            LoadJsonFile();

            if (ToDoList.Count == 0)
            {
                ToDoList.Add(new TaskItem("Приготовить покушать", new(2024, 01, 15), "Нет описания"));
                ToDoList.Add(new TaskItem("Поработать", new(2024, 01, 20), "Съездить на совещание в Москву"));
                ToDoList.Add(new TaskItem("Отдохнуть", new(2024, 01, 02), "Съездить в отпуск в Сочи"));
            }

            UpdateProgress();

            DeleteTaskCommand = new RelayCommand(DeleteSelectedTask, CanDeleteSelectedTask);
            SaveTasksTxtCommand = new RelayCommand(SaveTxtFile);
            ToggleTaskCompletionCommand = new RelayCommand(ToggleTaskCompletion);

            foreach (var item in ToDoList)
            {
                item.PropertyChanged += TaskItem_PropertyChanged;
            }
            ToDoList.CollectionChanged += ToDoList_CollectionChanged;
        }

        /// <summary>
        /// Обработчик события изменения коллекции ToDoList.
        /// Обновляет подписки на PropertyChanged для новых/удаленных элементов.
        /// </summary>
        private void ToDoList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TaskItem item in e.NewItems)
                {
                    item.PropertyChanged += TaskItem_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (TaskItem item in e.OldItems)
                {
                    item.PropertyChanged -= TaskItem_PropertyChanged;
                }
            }
            EndToDo();
        }

        /// <summary>
        /// Обработчик события изменения свойства TaskItem.
        /// Вызывается при изменении IsCompleted, чтобы обновить прогресс.
        /// </summary>
        private void TaskItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskItem.IsCompleted))
            {
                EndToDo();
            }
        }

        /// <summary>
        /// Вызывает событие PropertyChanged для указанного свойства.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Обновляет значения счетчиков выполненных и общих задач.
        /// </summary>
        public void UpdateProgress()
        {
            CompletedTasks = ToDoList.Count(t => t.IsCompleted);
            TotalTasks = ToDoList.Count;
        }

        /// <summary>
        /// Сохраняет список задач в JSON-файл.
        /// </summary>
        private void SaveJsonFile()
        {
            try
            {
                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                }
                string json = JsonConvert.SerializeObject(ToDoList, Formatting.Indented);

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
        /// Загружает список задач из JSON-файла.
        /// </summary>
        private void LoadJsonFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);

                    var loadedToDos = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(json);

                    ToDoList.Clear();
                    if (loadedToDos != null)
                    {
                        foreach (var item in loadedToDos)
                        {
                            ToDoList.Add(item);
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
                UpdateProgress();
            }
        }

        /// <summary>
        /// Сохраняет список задач в текстовый файл, используя SaveFileDialog.
        /// </summary>
        private void SaveTxtFile(object parameter)
        {
            if (ToDoList.Count == 0)
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

                foreach (var todoItem in ToDoList)
                {
                    sb.AppendLine($"{(todoItem.IsCompleted ? "✔" : "")}{todoItem.Title}");
                    sb.AppendLine();
                    sb.AppendLine($"{todoItem.Description}");
                    sb.AppendLine();
                    sb.AppendLine($"{todoItem.DueDate:dd.MM.yyyy}");
                    sb.AppendLine();
                    sb.AppendLine();
                }
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());
            }
        }

        /// <summary>
        /// Централизованный метод для обновления UI (прогресс) и сохранения данных.
        /// Вызывается после любых изменений в списке задач.
        /// </summary>
        public void EndToDo()
        {
            UpdateProgress();
            SaveJsonFile();
        }

        /// <summary>
        /// Удаляет выбранную задачу из списка.
        /// </summary>
        private void DeleteSelectedTask(object parameter)
        {
            if (SelectedTask != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить дело?",
                    "Удаление дела",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ToDoList.Remove(SelectedTask);
                    EndToDo();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дело для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Определяет, можно ли выполнить команду DeleteTaskCommand.
        /// </summary>
        public bool CanDeleteSelectedTask(object parameter)
        {
            return SelectedTask != null;
        }

        /// <summary>
        /// Переключает статус выполнения задачи.
        /// </summary>
        private void ToggleTaskCompletion(object parameter)
        {
            if (parameter is TaskItem task)
            {
                task.IsCompleted = !task.IsCompleted;
            }
        }
    }

    /// <summary>
    /// Вспомогательный класс для реализации ICommand, используемый для привязки команд в MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр RelayCommand.
        /// </summary>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда быть выполнена в текущем состоянии.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Выполняет логику команды.
        /// </summary>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
