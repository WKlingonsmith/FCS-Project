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
//using FCS_Funding.Models;

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
        public ObservableCollection<InKindItem> InKindItems { get; set; }
        public ObservableCollection<InKindService> InKindServices { get; set; }
        public ObservableCollection<EventsDataGrid> Events { get; set; }
        public ObservableCollection<ReportsDataGrid> Reports { get; set; }
        public ObservableCollection<AdminDataGrid> Admins { get; set; }
        public string PatientFilter { get; set; }
        public string PatientsSearchByEnum { get; set; }

        //Helper properties
        private bool ShouldLoadPatient { get; set; }
        private bool ShouldRefreshPatients { get; set; }

        public MainWindow()
        {
            
            //DGrid.ItemsSource = data;
            ShouldLoadPatient = true;
            ShouldRefreshPatients = false;
            InitializeComponent();
        }
        private void Patient_Grid(object sender, RoutedEventArgs e)
        {
            int index = this.SeachBy_Patients.SelectedIndex;
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
            var join1 = from patient in db.Patients
                        join patienthouse in db.PatientHouseholds on patient.HouseholdID equals patienthouse.HouseholdID
                        select new
                        {
                            pOQ = patient.PatientOQ,
                            FName = patient.PatientFirstName,
                            LName = patient.PatientLastName,
                            Gend = patient.PatientGender,
                            aGroup = patient.PatientAgeGroup,
                            ethn = patient.PatientEthnicity,
                            theTime = patient.NewClientIntakeHour,
                            head = patient.IsHead,
                            relation = patient.RelationToHead
                        };

            if (ShouldRefreshPatients)
            {
                PatientGrid.ItemsSource = join1.ToList();
                ShouldRefreshPatients = false;
                ShouldLoadPatient = false;
            }
            else
            {
                if (index == 0) //Search By Patient OQ 
                {
                    try
                    {
                        int value = Convert.ToInt32(PatientFilter);
                        var newjoin = from patient in join1
                                      where patient.pOQ.Equals(value)
                                      select patient;
                        PatientGrid.ItemsSource = newjoin.ToList();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Make sure you put a value in.");
                    }
                }
                else if (index == 1) //Search by First Name 
                {
                    var newjoin = from patient in join1
                                  where patient.FName.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 2) //Search by Last Name
                {
                    var newjoin = from patient in join1
                                  where patient.LName.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 3) //Search by Gender
                {
                    var newjoin = from patient in join1
                                  where patient.Gend.StartsWith(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 4) //Search by Age Group
                {
                    var newjoin = from patient in join1
                                  where patient.aGroup.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 5) //Search by Ethnicity
                {
                    var newjoin = from patient in join1
                                  where patient.ethn.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else //Did not select anything
                {
                    if (ShouldLoadPatient) //This is either refreshing the patient or initializing the patient
                    {
                        // ... Assign ItemsSource of DataGrid. 
                        var grid = sender as DataGrid;
                        if (grid == null)
                        {
                            PatientGrid.ItemsSource = join1.ToList();
                            ShouldLoadPatient = false;
                        }
                        else
                        {
                            grid.ItemsSource = join1.ToList();
                            ShouldLoadPatient = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Make sure you select a filter.");
                    }
                }
            }
            

        }
        private void Filter_Patients(object sender, RoutedEventArgs e)
        {
            Patient_Grid(sender, e);           
        }
        private void EditPatient(object sender, RoutedEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                DataGrid dg = sender as DataGrid;
                Patient p = (Patient)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdatePatient up = new UpdatePatient(p);
                up.Topmost = true;
                up.Show();
            }
        }

        private void Open_CreateNewPatient(object sender, RoutedEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                CreateNewPatient ch = new CreateNewPatient();
                ch.Topmost = true;
                ch.Show();
            }
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

        private void InKindItemGrid(object sender, RoutedEventArgs e)
        {
            InKindItem in1 = new InKindItem("TV", "Tom", "Fronberg", "HAFB", DateTime.Now, "Includes a remote");
            InKindItem in2 = new InKindItem("Couch", "Chris", "Johnson", "Clearfield Clinic", DateTime.Now, "Has a hole in it");
            InKindItems = new ObservableCollection<InKindItem>();
            InKindItems.Add(in1);
            InKindItems.Add(in2);
            var grid = sender as DataGrid;
            grid.ItemsSource = InKindItems;
        }

        private void Events_Grid(object sender, RoutedEventArgs e)
        {
            EventsDataGrid e1 = new EventsDataGrid(1234, DateTime.Now, DateTime.Now, "Fall Fundraiser", "Held at Marriot Ballroom");
            EventsDataGrid e2 = new EventsDataGrid(2345, DateTime.Now, DateTime.Now, "Spring Fundraiser", "Mayor of Ogden attending");
            EventsDataGrid e3 = new EventsDataGrid(3456, DateTime.Now, DateTime.Now, "Summer Fundraiser", "Focusing on mental health");
            EventsDataGrid e4 = new EventsDataGrid(1234, DateTime.Now, DateTime.Now, "Winter Fundraiser", "Give us money");
            Events = new ObservableCollection<EventsDataGrid>();
            Events.Add(e1);
            Events.Add(e2);
            Events.Add(e3);
            Events.Add(e4);
            var grid = sender as DataGrid;
            grid.ItemsSource = Events;

        }

        private void Reports_Grid(object sender, RoutedEventArgs e)
        {
            ReportsDataGrid r1 = new ReportsDataGrid("Summer Fund Raiser", "It was great");
            ReportsDataGrid r2 = new ReportsDataGrid("Fall Fund Raiser", "It was great");
            Reports = new ObservableCollection<ReportsDataGrid>();
            Reports.Add(r1);
            Reports.Add(r2);
            var grid = sender as DataGrid;
            grid.ItemsSource = Reports;
        }

        private void Admin_Grid(object sender, RoutedEventArgs e)
        {
            AdminDataGrid a1 = new AdminDataGrid("13224", "Billy", "Joel");
            AdminDataGrid a2 = new AdminDataGrid("12347", "Lionnel", "Messi");
            Admins = new ObservableCollection<AdminDataGrid>();
            Admins.Add(a1);
            Admins.Add(a2);
            var grid = sender as DataGrid;
            grid.ItemsSource = Admins;
        }

        private void InKindServiceGrid(object sender, RoutedEventArgs e)
        {
            DateTime start = new DateTime(2016, 2, 17, 8, 24, 32);
            DateTime end = new DateTime(2016, 2, 17, 16, 17, 8);
            InKindService s1 = new InKindService("Spencer", "Fronberg", "HAFB", start, end, 10.75M, "Coding");
            InKindServices = new ObservableCollection<InKindService>();
            InKindServices.Add(s1);
            var grid = sender as DataGrid;
            grid.ItemsSource = InKindServices;
        }

        private void Refresh_Patients(object sender, RoutedEventArgs e)
        {
            ShouldLoadPatient = true;
            ShouldRefreshPatients = true;
            Patient_Grid(sender, e);
        }
        

        
    }
}
