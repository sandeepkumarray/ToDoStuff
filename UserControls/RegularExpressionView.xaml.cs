using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegularExpressionView.xaml
    /// </summary>
    [ControlNameAttribute("Reg-Ex View")]
    public partial class RegularExpressionView : UserControl, IControlsInterface
    {
        public RegularExpressionView()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            Regex _filterRegex = new Regex(txtRegEx.Text.Trim(), RegexOptions.Compiled);
            StringBuilder sb = new StringBuilder();
            List<string> matchedValues = new List<string>();
            foreach (Match match in _filterRegex.Matches(txtText.Text.Trim()))
            {
                matchedValues.Add(match.Value);
            }
            sb.Append("new List<string>() { ");
            sb.Append(string.Join(",", matchedValues));
            sb.Append(" };");
            txtOut.Text = sb.ToString();
        }
    }
}
