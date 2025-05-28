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
        
        public List<ToDo> toDoList = new List<ToDo>();
        public DateTime defDate = new DateTime(2024,01,10);
        public string defDescription = "Описания нет";

        public MainWindow()
        {
            InitializeComponent();

            dateToDo.SelectedDate = defDate;
            descriptionToDo.Text = defDescription;

            toDoList.Add(new("Приготовить покушать", new(2024, 01, 15), "Нет описания"));
            toDoList.Add(new("Поработать", new(2024, 01, 20), "Съездить на совещание в Москву"));
            toDoList.Add(new("Отдохнуть", new(2024, 01, 02), "Съездить в отпуск в Сочи"));

            listToDo.ItemsSource = toDoList;

             
            
            groupBoxToDo.Visibility = Visibility.Collapsed;

            
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
            if (listToDo.SelectedItem != null)
            {
                ToDo taskForDelete = listToDo.SelectedItem as ToDo;
                if (taskForDelete != null)
                {
                    toDoList.Remove(taskForDelete);

                    UpdateListToDo();
                }
            }
            else
            {
                MessageBox.Show("ненене");
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dateToDo.SelectedDate != null)
            {
                ToDo newTask = new(titleToDo.Text, dateToDo.SelectedDate.Value, descriptionToDo.Text);
                toDoList.Add(newTask);

                titleToDo.Text = string.Empty;
                dateToDo.SelectedDate = defDate;
                descriptionToDo.Text = defDescription;

                UpdateListToDo();
            }
            else
            {
                MessageBox.Show("ненене");
            }
        }

        public void UpdateListToDo()
        {
            listToDo.ItemsSource = null;
            listToDo.ItemsSource = toDoList;
        }
    }
}