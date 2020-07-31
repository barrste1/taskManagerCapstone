using System;
using System.Collections.Generic;
using System.Text;

namespace _07312020_Task_List
{
    class Task
    {
        #region field
        private string _name;
        private string _description;
        private DateTime _dueDate;
        private bool _complete;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }
        public bool Complete
        {
            get { return _complete; }
            set { _complete = value; }
        }
        #endregion

        #region Constructors
        public Task() { }
        public Task(string Name, string Description, DateTime DueDate, bool Complete)
        {
            _name = Name;_description = Description; _dueDate = DueDate; _complete = Complete;
        }
        #endregion

        #region Methods
        public  void Completed()
        {
            _complete = !_complete;
        }
        #endregion
    }
}
