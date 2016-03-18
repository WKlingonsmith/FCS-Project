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
using FCS_Funding.Models;

namespace FCS_Funding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PatientGrid> Patients { get; set; }
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
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var join1 = from patient in db.Patients
                        join patienthouse in db.PatientHouseholds on patient.HouseholdID equals patienthouse.HouseholdID
                        select new PatientGrid
                        {
                            PatientOQ = patient.PatientOQ,
                            FirstName = patient.PatientFirstName,
                            LastName = patient.PatientLastName,
                            Gender = patient.PatientGender,
                            AgeGroup = patient.PatientAgeGroup,
                            Ethnicity = patient.PatientEthnicity,
                            Time = patient.NewClientIntakeHour,
                            IsHead = patient.IsHead,
                            RelationToHead = patient.RelationToHead
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
                                      where patient.PatientOQ.Equals(value)
                                      select patient;
                        PatientGrid.ItemsSource = newjoin.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("Make sure you put a value in.");
                    }
                }
                else if (index == 1) //Search by First Name 
                {
                    var newjoin = from patient in join1
                                  where patient.FirstName.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 2) //Search by Last Name
                {
                    var newjoin = from patient in join1
                                  where patient.LastName.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 3) //Search by Gender
                {
                    var newjoin = from patient in join1
                                  where patient.Gender.StartsWith(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 4) //Search by Age Group
                {
                    var newjoin = from patient in join1
                                  where patient.AgeGroup.Contains(PatientFilter)
                                  select patient;
                    PatientGrid.ItemsSource = newjoin.ToList();
                }
                else if (index == 5) //Search by Ethnicity
                {
                    var newjoin = from patient in join1
                                  where patient.Ethnicity.Contains(PatientFilter)
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
                        //MessageBox.Show("Make sure you select a filter.");
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
                PatientGrid p = (PatientGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdatePatient up = new UpdatePatient(p);
                up.TheHead.IsChecked = p.IsHead;
                up.Gender.SelectedIndex = Determine_GenderIndex(p.Gender);
                up.AgeGroup.SelectedIndex = Determine_AgeGroupIndex(p.AgeGroup);
                up.Ethnicity.SelectedIndex = Determine_EthnicGroupIndex(p.Ethnicity);
                //up.firstName = p.FirstName;
                //up.lastName = p.LastName;
                //up.patientOQ = p.PatientOQ;
                //up.relationToHead = p.RelationToHead;
                up.Topmost = true;
                up.Show();
            }
        }
        private void EditDonor(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                DataGrid dg = sender as DataGrid;
                DonorsDataGrid p = (DonorsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                if (p.DonorType == "Individual" )
                {
                    Models.FCS_FundingContext db = new Models.FCS_FundingContext();
                    //Open in individual view
                    Models.DonorContact query = (from doncontacts in db.DonorContacts
                                where doncontacts.DonorID == p.DonorID
                                select doncontacts).First();
                    UpdateIndividualDonor id = new UpdateIndividualDonor(p, query);
                    id.Show();
                    id.dType.SelectedIndex = 1;
                    id.oName.IsEnabled = false;
                    id.Topmost = true;
                }
                else if(p.DonorType == "Anonymous")
                {
                    Models.FCS_FundingContext db = new Models.FCS_FundingContext();
                    Models.DonorContact query = (from doncontacts in db.DonorContacts
                                                 where doncontacts.DonorID == p.DonorID
                                                 select doncontacts).First();
                    UpdateIndividualDonor id = new UpdateIndividualDonor(p, query);
                    id.Show();
                    id.UpdateIndDonor.IsEnabled = false;
                    id.dType.SelectedIndex = 2;
                    id.fName.IsEnabled = false;
                    id.lName.IsEnabled = false;
                    id.oName.IsEnabled = false;
                    id.donA1.IsEnabled = false;
                    id.donA2.IsEnabled = false;
                    id.cPhone.IsEnabled = false;
                    id.dCity.IsEnabled = false;
                    id.cPhone.IsEnabled = false;
                    id.dState.IsEnabled = false;
                    id.dZip.IsEnabled = false;
                    id.cEmail.IsEnabled = false;
                    id.Topmost = true;

                }
                else
                {
                    UpdateDonor up = new UpdateDonor(p);
                    //up.firstName = p.FirstName;
                    //up.lastName = p.LastName;
                    //up.patientOQ = p.PatientOQ;
                    //up.relationToHead = p.RelationToHead;
                    up.Show();
                    up.Topmost = true;
                }
            }
        }
        private int Determine_GenderIndex(string selection)
        {
            switch (selection)
            {
                case "Male":
                    return 0;
                case "Female":
                    return 1;
                case "Other":
                    return 2;
                default:
                    return 2;
            }
        }
        private int Determine_AgeGroupIndex(string selection)
        {
            switch (selection)
            {
                case "0-5":
                    return 0;
                case "6-11":
                    return 1;
                case "12-17":
                    return 2;
                case "18-23":
                    return 3;
                case "24-44":
                    return 4;
                case "45-54":
                    return 5;
                case "55-69":
                    return 6;
                case "70+":
                    return 7;
                default:
                    return 0;
            }
        }
        private int Determine_EthnicGroupIndex(string selection)
        {
            switch (selection)
            {
                case "African American":
                    return 0;
                case "Native/Alaskan":
                    return 1;
                case "Pacific Islander":
                    return 2;
                case "Asian":
                    return 3;
                case "Caucasian":
                    return 4;
                case "Hispanic":
                    return 5;
                case "Other":
                    return 6;
                default:
                    return 0;
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
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var join1 = from p in db.Purposes
                        join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                        join d in db.Donations on dp.DonationID equals d.DonationID
                        join dr in db.Donors on d.DonorID equals dr.DonorID
                        join gp in db.GrantProposals on dr.DonorID equals gp.DonorID
                        where gp.GrantStatus == "Accepted" && d.GrantProposalID == gp.GrantProposalID
                        select new GrantsDataGrid
                        {
                            GrantName = gp.GrantName,
                            DonationAmount = d.DonationAmount,
                            DonationAmountRemaining = d.DonationAmountRemaining,
                            DonationDate = d.DonationDate,
                            ExpirationDate = d.DonationExpirationDate,
                            PurposeName = p.PurposeName,
                            PurposeDescription = p.PurposeDescription,
                            PurposeID = p.PurposeID,
                            DonationID = d.DonationID,
                            DonorID = dr.DonorID,
                            GrantProposalID = gp.GrantProposalID
                        };
        
            GrantsDataGrid g1 = new GrantsDataGrid("Cross Charitable Foundation", 1024.25M, DateTime.Now, DateTime.Now, "Charitable Minds", "We wanted to donate", 1024.25M);
            Grants = new ObservableCollection<GrantsDataGrid>();
            Grants.Add(g1);

            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void Donor_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         where p.DonorType == "Anonymous" || p.DonorType == "Individual"
                         select new DonorsDataGrid
                         {
                             DonorID = p.DonorID,
                             DonorFirstName = dc.ContactFirstName,
                             DonorLastName = dc.ContactLastName,
                             DonorAddress1 = p.DonorAddress1,
                             OrganizationName = "",
                             DonorAddress2 = p.DonorAddress2,
                             DonorCity = p.DonorCity,
                             DonorState = p.DonorState,
                             DonorType = p.DonorType,
                             DonorZip = p.DonorZip
                         }).Union(
                        from p in db.Donors
                        where p.DonorType == "Organization" || p.DonorType == "Government"
                        select new DonorsDataGrid
                        {
                            DonorID = p.DonorID,
                            DonorFirstName = "",
                            DonorLastName = "",
                            DonorAddress1 = p.DonorAddress1,
                            OrganizationName = p.OrganizationName,
                            DonorAddress2 = p.DonorAddress2,
                            DonorCity = p.DonorCity,
                            DonorState = p.DonorState,
                            DonorType = p.DonorType,
                            DonorZip = p.DonorZip
                        });
            //DonorsDataGrid d1 = new DonorsDataGrid("Tom", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "", "Clinton", "Utah", "84015");
            //DonorsDataGrid d2 = new DonorsDataGrid("Spencer", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "652 West 800 North", "Clinton", "Utah", "84015");
            //Donors = new ObservableCollection<DonorsDataGrid>();
            //Donors.Add(d1);
            //Donors.Add(d2);
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void InKindItemGrid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         join d in db.Donations on  p.DonorID equals d.DonorID
                         join ki in db.In_Kind_Item on d.DonationID equals ki.DonationID
                         where p.DonorType == "Anonymous" || p.DonorType == "Individual"
                         select new InKindItem
                         {
                             DonorID = p.DonorID,
                             ItemID = ki.ItemID,
                             DonationID = d.DonationID,
                             ItemName = ki.ItemName,
                             DonorFirstName = dc.ContactFirstName,
                             DonorLastName = dc.ContactLastName,
                             OrganizationName = "",
                             DateRecieved = d.DonationDate,
                             Description = ki.ItemDescription
                         }).Union(
                        from p in db.Donors
                        join d in db.Donations on p.DonorID equals d.DonorID
                        join ki in db.In_Kind_Item on d.DonationID equals ki.DonationID
                        where p.DonorType == "Organization" || p.DonorType == "Government"
                        select new InKindItem
                        {
                            DonorID = p.DonorID,
                            ItemID = ki.ItemID,
                            DonationID = d.DonationID,
                            ItemName = ki.ItemName,
                            DonorFirstName = "",
                            DonorLastName = "",
                            OrganizationName = p.OrganizationName,
                            DateRecieved = d.DonationDate,
                            Description = ki.ItemDescription
                        });
            //DonorsDataGrid d1 = new DonorsDataGrid("Tom", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "", "Clinton", "Utah", "84015");
            //DonorsDataGrid d2 = new DonorsDataGrid("Spencer", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "652 West 800 North", "Clinton", "Utah", "84015");
            //Donors = new ObservableCollection<DonorsDataGrid>();
            //Donors.Add(d1);
            //Donors.Add(d2);
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
            //InKindItem in1 = new InKindItem("TV", "Tom", "Fronberg", "HAFB", DateTime.Now, "Includes a remote");
            //InKindItem in2 = new InKindItem("Couch", "Chris", "Johnson", "Clearfield Clinic", DateTime.Now, "Has a hole in it");
            //InKindItems = new ObservableCollection<InKindItem>();
            //InKindItems.Add(in1);
            //InKindItems.Add(in2);
            //var grid = sender as DataGrid;
            //grid.ItemsSource = InKindItems;
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
            //DateTime start = new DateTime(2016, 2, 17, 8, 24, 32);
            //DateTime end = new DateTime(2016, 2, 17, 16, 17, 8);
            //InKindService s1 = new InKindService("Spencer", "Fronberg", "HAFB", start, end, 10.75M, "Coding");
            //InKindServices = new ObservableCollection<InKindService>();
            //InKindServices.Add(s1);
            //var grid = sender as DataGrid;
            //grid.ItemsSource = InKindServices;
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         join d in db.Donations on p.DonorID equals d.DonorID
                         join ki in db.In_Kind_Service on d.DonationID equals ki.DonationID
                         where p.DonorType == "Anonymous" || p.DonorType == "Individual"
                         select new InKindService
                         {
                             DonorID = p.DonorID,
                             DonationID = d.DonationID,
                             ServiceID = ki.ServiceID,
                             DonorFirstName = dc.ContactFirstName,
                             DonorLastName = dc.ContactLastName,
                             StartDateTime = ki.StartDateTime,
                             EndDateTime = ki.EndDateTime,
                             RatePerHour = ki.RatePerHour,
                             ServiceDescription = ki.ServiceDescription,
                             Length = ki.ServiceLength,
                             Value = ki.ServiceValue
                         });
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void Refresh_Patients(object sender, RoutedEventArgs e)
        {
            ShouldLoadPatient = true;
            ShouldRefreshPatients = true;
            Patient_Grid(sender, e);
        }

        private void Refresh_Donors(object sender, RoutedEventArgs e)
        {

        }

        private void Open_CreateNewDonor(object sender, RoutedEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                CreateNewDonor ch = new CreateNewDonor();
                //ch.Topmost = true;
                ch.Show();
            }
        }

        private void Open_CreateGrantProposal(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                CreateGrantProposal cgp = new CreateGrantProposal();
                cgp.Show();
            }
            else
            {
                MessageBox.Show("Close other windows");
            }
        }

        private void Open_ViewGrantProposals(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                ViewGrantProposals vgp = new ViewGrantProposals();
                vgp.Show();
            }
            else
            {
                MessageBox.Show("Close other windows");
            }
        }

        private void EditGrant(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                DataGrid dg = sender as DataGrid;
                GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateGrant up = new UpdateGrant(p);
                up.DonationDate.SelectedDate = p.DonationDate;
                up.DonationExpirationDate.SelectedDate = p.ExpirationDate;
                up.Topmost = true;
                up.Show();
            }
        }

        private void Add_InKind_Item(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                AddInKindItem iki = new AddInKindItem();
                iki.Show();
                //iki.Topmost = true;
                iki.Organization.IsEnabled = false;
            }
        }
<<<<<<< HEAD

        private void Add_InKind_Service(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                AddInKindService iks = new AddInKindService();
                iks.Show();
                //iks.Topmost = true;
                iks.AMPM_End.SelectedIndex = 0;
                iks.AMPM_Start.SelectedIndex = 0;             
            }
        }

        private void Update_InKindItem(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                DataGrid dg = sender as DataGrid;
                InKindItem p = (InKindItem)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateInKindItem up = new UpdateInKindItem(p);
                up.DateRecieved.SelectedDate = p.DateRecieved;
                up.Topmost = true;
                up.Show();
            }
        }

        private void Update_InKindService(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1)
            {
                DataGrid dg = sender as DataGrid;
                InKindService p = (InKindService)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateInKindService up = new UpdateInKindService(p);
                up.DateRecieved.SelectedDate = p.StartDateTime;
                up.Topmost = true;

                if (p.StartDateTime.Hour >= 12)
                {
                    up.AMPM_Start.SelectedIndex = 1;
                }
                else
                {
                    up.AMPM_Start.SelectedIndex = 0;
                }
                if (p.EndDateTime.Hour >= 12)
                {
                    up.AMPM_End.SelectedIndex = 1;
                }
                else
                {
                    up.AMPM_End.SelectedIndex = 0;
                }


                up.Show();
            }
        }
=======
>>>>>>> parent of 6022924... Starting the event form I think?
    }
}