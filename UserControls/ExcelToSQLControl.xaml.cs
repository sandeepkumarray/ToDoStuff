using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using ToDoStuff.Model;
using YamlDotNet.RepresentationModel;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for ExcelToSQLControl.xaml
    /// </summary>
    [ControlNameAttribute("Excel To SQL")]
    public partial class ExcelToSQLControl : UserControl, IControlsInterface
    {
        public ExcelToSQLControl()
        {
            InitializeComponent();
            this.DisplayName = "Excel To SQL";
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnReadExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtExcelPath.Text))
                {
                    string sqlstring = ProcessExcelFile(txtExcelPath.Text.Trim());
                    FileUtility.SaveDataToFile(@"D:\com.sandeep.org\com.sandeep.init\Armour\notebook.ai\result.txt", sqlstring);
                }
                else
                    MessageBox.Show("Provide Excel File Path!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ProcessExcelFile(string FilePath)
        {
            string result = "";
            try
            {
                ExcelReader reader = new ExcelReader();
                DataTable tbl = reader.ReadCSVasDataTable(FilePath);
                if (tbl != null)
                {
                    SQLItemModel sqlItemModel = new SQLItemModel(tbl.TableName);
                    if (tbl.Columns != null && tbl.Columns.Count > 0)
                    {
                        foreach (DataColumn item in tbl.Columns)
                        {
                            sqlItemModel.Columns.Add(item);
                        }
                    }
                    SqlPhraseParser sqlPhraseParser = new SqlPhraseParser();
                    result = sqlPhraseParser.ParseModel(sqlItemModel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            return result;
        }

        private void btnProcessFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFolderPath.Text))
                {
                    string folderPath = txtFolderPath.Text.Trim();
                    string[] files = Directory.GetFiles(folderPath);
                    foreach (var item in files)
                    {
                        string sqlstring = ProcessExcelFile(item);
                        FileUtility.SaveDataToFile(System.IO.Path.Combine(folderPath, "result" + DateTime.Now.ToString("_dd_MM_yyyy_hh_mm_ss") + ".txt"), sqlstring);
                    }
                    MessageBox.Show("All files processed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Folder doesn't exist!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
