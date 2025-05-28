using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTaskManeger
{
    public class ToDo
    {
        private string _title;
        private string _description;
        private DateTime _date;

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

        public ToDo(string title, string description)
        {
            Title = title;
            Description = description;
        }
        public ToDo(string title, DateTime date, string description)
        {
            Title = title;
            Date = date;
            Description = description;
        }
    }
}
