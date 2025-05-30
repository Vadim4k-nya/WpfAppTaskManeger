using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfAppTaskManeger
{
    public class ToDo
    {
        private string _title;
        private DateTime _date;
        private string _description;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ToDo()
        {
            Title = "";
            Date = new DateTime();
            Description = "";
        }

        public ToDo(string title, DateTime date, string description)
        {
            Title = title;
            Date = date;
            Description = description;
        }
    }
}
