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

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static List<ToDo> toDoList = new List<ToDo>();

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
            if (listToDo.SelectedItem != null)
            {
                listToDo.CancelEdit();
                ToDo taskForDelete = listToDo.SelectedItem as ToDo;
                if (taskForDelete != null)
                {
                    toDoList.Remove(taskForDelete);
                    listToDo.Items.Refresh();
                    EndToDo();
                }
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
            
            if (listToDo.SelectedItem != null)
            {
                int indexOfSelectedItem = toDoList.IndexOf(listToDo.SelectedItem as ToDo);
                toDoList[indexOfSelectedItem].Doing = true;
            }
            EndToDo();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            
            if (listToDo.SelectedItem != null)
            {
                int indexOfSelectedItem = toDoList.IndexOf(listToDo.SelectedItem as ToDo);
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
    }
}