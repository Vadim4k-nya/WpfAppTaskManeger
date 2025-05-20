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
        
        

        public MainWindow()
        {
            InitializeComponent();

            dateToDo.SelectedDate = new DateTime(2024,01,10);
            descriptionToDo.Text = "Описания нет";

            List<ToDo> toDoList = new List<ToDo>();
            toDoList.Add(new("Приготовить покушать", new(2024, 01, 15), "Нет описания"));
            toDoList.Add(new("Поработать", new(2024, 01, 20), "Съездить на совещание в Москву"));
            toDoList.Add(new("Отдохнуть", new(2024, 01, 02), "Съездить в отпуск в Сочи"));

            
            
            listToDo.ItemsSource = toDoList;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            groupBoxToDo.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            groupBoxToDo.Visibility = Visibility.Collapsed;
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            ToDo taskForDelete = listToDo.SelectedItems as ToDo;
            if (taskForDelete != null)
            {
                listToDo.SelectedItems.Remove(taskForDelete);
                listToDo.ItemsSource = null;
            }
            else
            {
                MessageBox.Show("ненене");
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
        //    ToDo newTask = new (titleToDo.Text, dateToDo.SelectedDate.Value, descriptionToDo.Text);
        //    toDoList.Add(newTask);
            
        //    titleToDo.Text = string.Empty;
        //    dateToDo.SelectedDate = null;
        //    descriptionToDo.Text = string.Empty;

        //    listToDo.ItemsSource = null;
        //    listToDo.ItemsSource = toDoList;
        }
    }
}