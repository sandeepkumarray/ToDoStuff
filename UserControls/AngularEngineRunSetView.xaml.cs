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
using ToDoStuff.Engine;
using ToDoStuff.Helpers;
using Newtonsoft.Json;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for AngularEngineRunSetView.xaml
    /// </summary>
    [ControlNameAttribute("Angular Engine Run")]
    public partial class AngularEngineRunSetView : UserControl, IControlsInterface
    {
        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }
        public AngularProjectEngine engine;
        public AngularEngineRunSetView()
        {
            InitializeComponent();
            engine = new AngularProjectEngine();
            this.rtbSettingsJson.AppendText(Newtonsoft.Json.JsonConvert.SerializeObject(engine.angularProjectSetting, Formatting.Indented));
        }

        private void btnBrowseJson_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            string userUsings = new TextRange(rtbSettingsJson.Document.ContentStart, rtbSettingsJson.Document.ContentEnd).Text;

            if ((AngularProjectEngine)(this.DataContext) != null)
                engine = (AngularProjectEngine)(this.DataContext);

            var dataStore = engine.DataStore;

            if (!string.IsNullOrEmpty(userUsings))
            {
                engine = new AngularProjectEngine(userUsings);
            }

            engine.SetDataStore(dataStore).Initialize().Process();

            MessageBox.Show("Complete");
        }
    }
}
