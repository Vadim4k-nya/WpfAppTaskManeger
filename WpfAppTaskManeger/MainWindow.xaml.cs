using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static List<ToDo> toDoList = new List<ToDo>();
        private readonly string _filePath = "/Files/todo.json";
        private readonly string _folderPath = "/Files";

        public MainWindow()
        {
            InitializeComponent();
            
            toDoList.Add(new("Приготовить покушать", new(2024, 01, 15), "Нет описания"));
            toDoList.Add(new("Поработать", new(2024, 01, 20), "Съездить на совещание в Москву"));
            toDoList.Add(new("Отдохнуть", new(2024, 01, 02), "Съездить в отпуск в Сочи"));

            listToDo.ItemsSource = toDoList;
            EndToDo();

        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            ToDo taskToDelete = (sender as Button).DataContext as ToDo;
            if (taskToDelete != null)
            {
                toDoList.Remove(taskToDelete);
                listToDo.Items.Refresh();
                EndToDo();
            }
            else
            {
                MessageBox.Show("ненене");
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddToDo addToDoWindow = new AddToDo();

            addToDoWindow.Owner = this; 

            addToDoWindow.Show();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int indexOfSelectedItem = toDoList.IndexOf((sender as CheckBox).DataContext as ToDo);
            toDoList[indexOfSelectedItem].Doing = true;
            EndToDo();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            
            if (listToDo.SelectedItem != null)
            {
                int indexOfSelectedItem = toDoList.IndexOf((sender as CheckBox).DataContext as ToDo);
                toDoList[indexOfSelectedItem].Doing = false;
            }
            EndToDo();
        }

        public void EndToDo()
        {
            progressToDo.Minimum = 0;
            progressToDo.Maximum = listToDo.Items.Count;

            int cmpltTaskCount = 0;

            
            foreach (var item in toDoList)
            {
                if (item.Doing)
                {
                    cmpltTaskCount++;
                }
            }

            progressToDo.Value = cmpltTaskCount;
            progressTextToDo.Text = $"{cmpltTaskCount}/{listToDo.Items.Count}";
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveTxtFile();
        }

        private void SaveTxtFile()
        {
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
                    sb.AppendLine($"Заголовок: {todoItem.Title}");
                    sb.AppendLine($"Описание: {todoItem.Description}");
                    sb.AppendLine($"Дата: {todoItem.Date:dd.MM.yyyy}");
                    sb.AppendLine($"Выполнено: {(todoItem.Doing ? "Да" : "Нет")}");
                    sb.AppendLine("--------------------------------------------------");
                }
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());

            }
        }

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
        private void LoadJsonFile()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);

                var loadedToDos = JsonConvert.DeserializeObject<ObservableCollection<ToDo>>(json);

                toDoList.Clear();
                if (loadedToDos != null)
                {
                    foreach (var item in loadedToDos)
                    {
                        toDoList.Add(item);
                    }
                }
            }
            EndToDo(); 
        }
    }
}