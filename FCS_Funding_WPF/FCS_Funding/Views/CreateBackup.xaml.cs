using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateBackup.xaml
    /// </summary>
    public partial class CreateBackup : Window
    {
        public string BackupName { get; set; }
        public CreateBackup()
        {
            InitializeComponent();
        }

        private void Create_Backup(object sender, RoutedEventArgs e)
        {
            //if (BackupName != null && BackupName != "")
            //{
            //    //var _assembly = System.Reflection.Assembly
            //    //.GetExecutingAssembly().GetName().CodeBase;
            //    var _assembly = System.Reflection.Assembly
            //       .GetExecutingAssembly().Location;


            //    var _path = System.IO.Path.GetDirectoryName(_assembly);
            //    string newPath = _path.Replace("\\FCS_Funding.exe", "");
            //    string pathString2 = newPath + "\\" + BackupName;
            //    MessageBox.Show(pathString2);
            //    System.IO.Directory.CreateDirectory(pathString2);
            //    //File.Copy(source , destination);
            //    string FCS_Funding_mdf_From = newPath + "\\FCS_Funding.mdf";
            //    string FCS_Funding_Log_mdf_From = newPath + "\\FCS_Funding_log.ldf";

            //    string FCS_Funding_mdf_To = pathString2 + "\\FCS_Funding.mdf";
            //    string FCS_Funding_Log_mdf_To = pathString2 + "\\FCS_Funding_log.ldf";

            //    File.Copy(FCS_Funding_mdf_From, FCS_Funding_mdf_To);
            //    File.Copy(FCS_Funding_Log_mdf_From, FCS_Funding_Log_mdf_To);
            //}
        }
    }
}
