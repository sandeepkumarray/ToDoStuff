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

namespace ToDoStuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadControl();
        }

        private void LoadControl()
        {
            List<Type> AllControls = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IControlsInterface).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x=>x).ToList();

            MenuItem controlsMenu = new MenuItem();
            controlsMenu.Header = "Controls";

            foreach (var item in AllControls)
            {
                string DisplayName = item.CustomAttributes.Select(x => x.ConstructorArguments[0].Value).First().ToString();
                MenuItem controlMenu = new MenuItem();
                controlMenu.Header = DisplayName;
                controlMenu.Tag = item;
                controlMenu.Click += ControlMenu_Click;
                controlsMenu.Items.Add(controlMenu);
            }

            MainMenu.Items.Add(controlsMenu);
        }

        private void ControlMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            var win = (IControlsInterface)Activator.CreateInstance((Type)menu.Tag);
            controlContainer.Content = win;
        }

        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void restartMenu_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }
    }
}
