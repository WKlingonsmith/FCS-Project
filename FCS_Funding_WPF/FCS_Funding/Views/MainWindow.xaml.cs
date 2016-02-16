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
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<GrantsDataGrid> Grants { get; set; }
        public ObservableCollection<DonorsDataGrid> Donors { get; set; }
        public ObservableCollection<InKindDonation> InKindDonations { get; set; }
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
            Patients = new ObservableCollection<Patient>();
            Patients.Add(p1);
            Patients.Add(p2);
            Patients.Add(p3);
            Patients.Add(p4);
            Patients.Add(p5);
            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = Patients;
        }
        private void EditPatient(object sender, RoutedEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            
            MessageBox.Show("HI");
        }

        private void Open_CreateNewPatient(object sender, RoutedEventArgs e)
        {
            CreateNewPatient ch = new CreateNewPatient();
            ch.Show();
        }

        private void Grants_Grid(object sender, RoutedEventArgs e)
        {
            GrantsDataGrid g1 = new GrantsDataGrid("Cross Charitable Foundation", 1024.25M, DateTime.Now, "Charitable Minds", "We wanted to donate");
            GrantsDataGrid g2 = new GrantsDataGrid("New Charity", 124.25M, DateTime.Now, "Charitable Minds", "We wanted to donate");
            GrantsDataGrid g3 = new GrantsDataGrid("Cross Charitable Foundation", 104.25M, DateTime.Now, "Charitable Minds", "We wanted to donate");
            GrantsDataGrid g4 = new GrantsDataGrid("Cross Charitable Foundation", 102.25M, DateTime.Now, "Charitable People", "We wanted to donate");
            GrantsDataGrid g5 = new GrantsDataGrid("Cross Charitable Foundation", 241.25M, DateTime.Now, "C", "We wanted to donate");
            GrantsDataGrid g6 = new GrantsDataGrid("Cross Charitable Foundation", 254.25M, DateTime.Now, "Chari", "We wanted to donate");
            GrantsDataGrid g7 = new GrantsDataGrid("Cross Charitable Foundation", 184.25M, DateTime.Now, "Charitds", "We wanted to donate");
            GrantsDataGrid g8 = new GrantsDataGrid("Cross Charitable Foundation", 1024.25M, DateTime.Now, "Charitable Minds", "We wanted to donate");
            GrantsDataGrid g9 = new GrantsDataGrid("New Charity", 124.25M, DateTime.Now, "Charitable Minds", "We wanted to donate");
            GrantsDataGrid g13 = new GrantsDataGrid("Cross Charitable Foundation", 104.25M, DateTime.Now, "Charitable Minds", "We wanted to donate");
            GrantsDataGrid g14 = new GrantsDataGrid("Cross Charitable Foundation", 102.25M, DateTime.Now, "Charitable People", "We wanted to donate");
            GrantsDataGrid g15 = new GrantsDataGrid("Cross Charitable Foundation", 241.25M, DateTime.Now, "C", "We wanted to donate");
            GrantsDataGrid g16 = new GrantsDataGrid("Cross Charitable Foundation", 254.25M, DateTime.Now, "Chari", "We wanted to donate");
            GrantsDataGrid g17 = new GrantsDataGrid("Cross Charitable Foundation", 184.25M, DateTime.Now, "Charitds", "We wanted to donate");
            Grants = new ObservableCollection<GrantsDataGrid>();
            Grants.Add(g1);
            Grants.Add(g2);
            Grants.Add(g3);
            Grants.Add(g4);
            Grants.Add(g5);
            Grants.Add(g6);
            Grants.Add(g7);
            Grants.Add(g8);
            Grants.Add(g9);
            Grants.Add(g13);
            Grants.Add(g14);
            Grants.Add(g15);
            Grants.Add(g16);
            Grants.Add(g17);
            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = Grants;
        }

        private void Donor_Grid(object sender, RoutedEventArgs e)
        {
            DonorsDataGrid d1 = new DonorsDataGrid("Tom", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "", "Clinton", "Utah", "84015");
            DonorsDataGrid d2 = new DonorsDataGrid("Spencer", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "652 West 800 North", "Clinton", "Utah", "84015");
            Donors = new ObservableCollection<DonorsDataGrid>();
            Donors.Add(d1);
            Donors.Add(d2);
            var grid = sender as DataGrid;
            grid.ItemsSource = Donors;
        }

        private void InKindGrid(object sender, RoutedEventArgs e)
        {
            InKindDonation in1 = new InKindDonation("-", "Tom Fronberg", "HAFB", DateTime.Now, "Helped Write Code", 5M, 11.25M);
            InKindDonation in2 = new InKindDonation("Couch", "-", "HAFB", DateTime.Now, "Couch");
            InKindDonations = new ObservableCollection<InKindDonation>();
            InKindDonations.Add(in1);
            InKindDonations.Add(in2);
            var grid = sender as DataGrid;
            grid.ItemsSource = InKindDonations;
        }

       
    }
}
