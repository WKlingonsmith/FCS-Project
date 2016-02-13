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
using FCS_Funding.Views;
using FCS_DataTesting;
using System.Collections.ObjectModel;

namespace FCS_Funding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Patient> data { get; set; }
        public MainWindow()
        {
            //ItemsSource="{Binding Source=data}"
            Patient p1 = new Patient("Spencer", 2546, "Loves basketball");
            Patient p2 = new Patient("Tom", 2547, "Hate basketball");
            Patient p3 = new Patient("Chris", 2548, "Loves football");
            data = new ObservableCollection<Patient>();
            data.Add(p1); 
            data.Add(p2);
            data.Add(p3);
            DGrid.ItemsSource = null;
            DGrid.ItemsSource = data;
            
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateNewPatient ch = new CreateNewPatient();
            ch.Show();
        }

       
    }
}
