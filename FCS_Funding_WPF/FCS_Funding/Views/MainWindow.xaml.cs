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
            
            //DGrid.ItemsSource = data;
            
            InitializeComponent();
        }
        private void Patient_Grid(object sender, RoutedEventArgs e)
        {
            //ItemsSource="{Binding Source=data}"
            Patient p1 = new Patient(2546, "Chri", "Fronberg", "Male", "18-24", "White", DateTime.Now, false, "Son", "Loves basketball");
            Patient p2 = new Patient(2546, "Eric", "Fronberg", "Male", "18-24", "White", DateTime.Now, true, "Head", "Loves basketball");
            Patient p3 = new Patient(2546, "Mandy", "Fronberg", "Female", "18-24", "White", DateTime.Now, false, "Wife", "Loves basketball");
            Patient p4 = new Patient(2546, "Vince", "Fronberg", "Male", "18-24", "White", DateTime.Now, true, "Head", "Loves basketball");
            Patient p5 = new Patient(2546, "Tom", "Fronberg", "Male", "18-24", "White", DateTime.Now, true, "Head", "Loves basketball");
            data = new ObservableCollection<Patient>();
            data.Add(p1);
            data.Add(p2);
            data.Add(p3);
            data.Add(p4);
            data.Add(p5);
            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = data;
        }
        private void EditPatient(object sender, RoutedEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            
            MessageBox.Show("HI");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateNewPatient ch = new CreateNewPatient();
            ch.Show();
        }

       
    }
}
