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

namespace WpfAppTaskManeger
{
    /// <summary>
    /// Логика взаимодействия для AddToDo.xaml
    /// </summary>
    public partial class AddToDo : Window
    {
        public DateTime defDate = new DateTime(2024, 01, 10);
        public string defDescription = "Описания нет";

        public AddToDo()
        {
            InitializeComponent();
            dateToDo.SelectedDate = defDate;
            descriptionToDo.Text = defDescription;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (dateToDo.SelectedDate != null)
            {
                MainWindow.toDoList.Add(new ToDo(titleToDo.Text, dateToDo.SelectedDate.Value, descriptionToDo.Text));

                (this.Owner as MainWindow).listToDo.Items.Refresh();

                titleToDo.Text = string.Empty;
                dateToDo.SelectedDate = defDate;
                descriptionToDo.Text = defDescription;

                this.Close();
            }
            else
            {
                MessageBox.Show("ненене");
            }
            
        }

    }
}
