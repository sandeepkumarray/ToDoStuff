using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStuff
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    class ControlNameAttribute : Attribute
    {
        private string _value;
        public string Value { get => _value; set => _value = value; }

        public ControlNameAttribute(string value)
        {
            this._value = value;
        }

    }
}
