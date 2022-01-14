using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ToDoStuff
{
    public interface IControlsInterface
    {
        string DisplayName { get; set; }
        UserControl LoadControl();
    }
}
