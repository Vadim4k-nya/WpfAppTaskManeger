using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppTaskManeger.Model;

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Логика взаимодействия для AddToDo.xaml
    /// </summary>
    /// 
    public partial class AddToDo : Window
    {
        public DateTime defDate = DateTime.Today;
        public string defDescription = "Описания нет";

        // Declare custom command for saving a new ToDo in this window
        public static readonly RoutedCommand SaveNewToDoCommand = new RoutedCommand();

        public AddToDo()
        {
            InitializeComponent();
            dateToDo.SelectedDate = defDate;
            descriptionToDo.Text = defDescription;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveNewToDoItem();
        }

        private void SaveNewToDo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveNewToDoItem();
        }

        private void SaveNewToDoItem()
        {
            if (dateToDo.SelectedDate != null && !string.IsNullOrWhiteSpace(titleToDo.Text))
            {
                MainWindow.toDoList.Add(new TaskItem(titleToDo.Text, dateToDo.SelectedDate.Value, descriptionToDo.Text));

                titleToDo.Text = string.Empty;
                dateToDo.SelectedDate = defDate;
                descriptionToDo.Text = defDescription;

                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните название и выберите дату.", "Ошибка добавления", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
