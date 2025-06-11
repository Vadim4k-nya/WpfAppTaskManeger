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
        private bool _doing;

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
        public bool Doing
        {
            get { return _doing; }
            set { _doing = value; }
        }

        public ToDo()
        {
            Title = "Title";
            Date = DateTime.Now;
            Description = "Description";
            Doing = true;
        }

        public ToDo(string title, DateTime date, string description)
        {
            Title = title;
            Date = date;
            Description = description;
        }

        public ToDo(string title, DateTime date, string description, bool doing) 
            : this(title, date, description)
        {
            Doing = doing;
        }
    }
}
