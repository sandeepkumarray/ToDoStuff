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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoStuff.UserControls
{

    /// <summary>
    /// Interaction logic for RailsSchemaToSQLControl.xaml
    /// </summary>

    [ControlNameAttribute("Rails Schema To SQL")]
    public partial class RailsSchemaToSQLControl : UserControl, IControlsInterface
    {
        public RailsSchemaToSQLControl()
        {
            InitializeComponent();
            this.DisplayName = "Rails Schema To SQL";
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            string path = txtPath.Text;
            ParseRailsSchemaToSQL objParseRailsSchemaToSQL = new ParseRailsSchemaToSQL(new Uri(path));
            string txtResult = objParseRailsSchemaToSQL.ConvertToSQL();
            rtbResult.AppendText(txtResult);
        }
    }
}
