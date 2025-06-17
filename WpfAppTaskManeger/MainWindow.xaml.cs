using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<ToDo> toDoList = new List<ToDo>();

        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        private readonly string _filePath; 

        public static readonly RoutedCommand DeleteToDoCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();

            _filePath = Path.Combine(_folderPath, "todo.json");

            if (toDoList.Count == 0)
            {
                toDoList.Add(new ToDo("Приготовить покушать", new(2024, 01, 15), "Нет описания"));
                toDoList.Add(new ToDo("Поработать", new(2024, 01, 20), "Съездить на совещание в Москву"));
                toDoList.Add(new ToDo("Отдохнуть", new(2024, 01, 02), "Съездить в отпуск в Сочи"));
            }

            listToDo.ItemsSource = toDoList;
            listToDo.Items.Refresh();

            EndToDo();
        }

        // кнопачке

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenAddToDoWindow();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTxtFile();
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            ToDo taskToDelete = (sender as Button)?.DataContext as ToDo;
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
                    listToDo.Items.Refresh();
                    EndToDo();
                    SaveJsonFile();
                }
            }
            else
            {
                MessageBox.Show("Не удалось определить дело для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ToDo? checkedTask = (sender as CheckBox)?.DataContext as ToDo;
            if (checkedTask != null)
            {
                checkedTask.Doing = true;
                listToDo.Items.Refresh();
                EndToDo();
                SaveJsonFile();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ToDo? uncheckedTask = (sender as CheckBox)?.DataContext as ToDo;
            if (uncheckedTask != null)
            {
                uncheckedTask.Doing = false;
                listToDo.Items.Refresh();
                EndToDo();
                SaveJsonFile();
            }
        }

        public void EndToDo()
        {
            progressToDo.Minimum = 0;
            progressToDo.Maximum = toDoList.Count();

            int cmpltTaskCount = 0;

            foreach (var item in toDoList)
            {
                if (item.Doing)
                {
                    cmpltTaskCount++;
                }
            }

            progressToDo.Value = cmpltTaskCount;
            progressTextToDo.Text = $"{cmpltTaskCount}/{toDoList.Count()}";
        }

        // Операции Файлов

        //// ТхТ

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

        
        //// джсын
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadJsonFile();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveJsonFile();
        }

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

        private void LoadJsonFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);

                    var loadedToDos = JsonConvert.DeserializeObject<List<ToDo>>(json);

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
                EndToDo();
            }
        }

        

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenAddToDoWindow();
        }

        private void OpenAddToDoWindow()
        {
            AddToDo addToDoWindow = new AddToDo();
            addToDoWindow.Owner = this;
            addToDoWindow.ShowDialog();

            listToDo.Items.Refresh();
            EndToDo();
            SaveJsonFile();
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveTxtFile();
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ToDo? taskToDelete = listToDo.SelectedItem as ToDo;
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
                    listToDo.Items.Refresh();
                    EndToDo();
                    SaveJsonFile();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дело для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}