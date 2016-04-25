using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using FCS_Funding.Views;
using FCS_DataTesting;
using System.Collections.ObjectModel;
using FCS_Funding.Models;
//using System.Windows.Automation.Peers;

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
        //Accessablity
        private string StaffDBRole { get; set; }
        FCS_DBModel db;
        public MainWindow(string StaffRole)
        {
            StaffDBRole = StaffRole;
            //DGrid.ItemsSource = data;
            ShouldLoadPatient = true;
            ShouldRefreshPatients = false;
            InitializeComponent();
        }
        private void Patient_Grid(object sender, RoutedEventArgs e)
        {
            int index = this.SeachBy_Clients.SelectedIndex;
            db = new FCS_DBModel(); 
            var join1 = from patient in db.Patients
                        join patienthouse in db.PatientHouseholds on patient.HouseholdID equals patienthouse.HouseholdID
                        select new PatientGrid
                        {
                            PatientOQ = patient.PatientOQ,
                            PatientID = patient.PatientID,
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
                        string value = PatientFilter;
                        var newjoin = from patient in join1
                                      where patient.PatientOQ.Contains(value)
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
                            //PatientGrid.ItemsSource = join1.ToList();
                            grid.ItemsSource = join1.ToList();
                            ShouldLoadPatient = false;
                        }
                    }
                    else
                    {
                        PatientGrid.ItemsSource = join1.ToList();
                        //MessageBox.Show("Make sure you select a filter.");
                    }
                }
                GC.Collect();
            }    
        }
        private void Filter_Patients(object sender, RoutedEventArgs e)
        {
            Patient_Grid(sender, e);           
        }

        private void EditPatient(object sender, RoutedEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count < 2)
            {
                try
                {
                    DataGrid dg = sender as DataGrid;
                    PatientGrid p = (PatientGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                    UpdatePatient up = new UpdatePatient(p);
                    if (StaffDBRole != "Admin")
                    {
                        up.DeleteClien.IsEnabled = false;
                    }
                    if(StaffDBRole == "Basic")
                    {
                        up.UpPatient.IsEnabled = false;
                        up.DeleteClien.IsEnabled = false;
                        up.AddSession.IsEnabled = false;
                    }
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
                    //DGrid.ItemsSource = data;
                }
                catch
                {
                    MessageBox.Show("You already deleted this Patient");
                }
            }
        }
        private void EditDonor(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int Count = Application.Current.Windows.Count;
                if (Count < 2 && StaffDBRole != "Basic")
                {
                    DataGrid dg = sender as DataGrid;
                    DonorsDataGrid p = (DonorsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                    if (p.DonorType == "Individual")
                    {
                        db = new FCS_DBModel(); 
                        //Open in individual view
                        Models.DonorContact query = (from doncontacts in db.DonorContacts
                                                     where doncontacts.DonorID == p.DonorID
                                                     select doncontacts).First();
                        UpdateIndividualDonor id = new UpdateIndividualDonor(p, query, StaffDBRole);
                        if (StaffDBRole != "Admin")
                        {
                            id.DeleteIndDonor.IsEnabled = false;
                        }
                        id.Show();
                        id.dType.SelectedIndex = 1;
                        id.oName.IsEnabled = false;
                        id.Topmost = true;
                    }
                    else if (p.DonorType == "Anonymous")
                    {
                        db = new FCS_DBModel(); 
                        Models.DonorContact query = (from doncontacts in db.DonorContacts
                                                     where doncontacts.DonorID == p.DonorID
                                                     select doncontacts).First();
                        UpdateIndividualDonor id = new UpdateIndividualDonor(p, query, StaffDBRole);
                        if (StaffDBRole != "Admin")
                        {
                            id.DeleteIndDonor.IsEnabled = false;
                        }
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
                        UpdateDonor up = new UpdateDonor(p, StaffDBRole);
                        if (StaffDBRole != "Admin")
                        {
                            up.DeleteDon.IsEnabled = false;
                        }
                        //up.firstName = p.FirstName;
                        //up.lastName = p.LastName;
                        //up.patientOQ = p.PatientOQ;
                        //up.relationToHead = p.RelationToHead;
                        up.Show();
                        up.Topmost = true;
                    }

                }
            }
            catch
            {
                MessageBox.Show("You already deleted this Donor");
            }
        }
        private void EditGrant(object sender, MouseButtonEventArgs e)
        {
            db = new FCS_DBModel(); 
            int Count = Application.Current.Windows.Count;
            if (Count < 2 && StaffDBRole != "Basic")
            {
                DataGrid dg = sender as DataGrid;
                GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateGrant up = new UpdateGrant(p);
                if (StaffDBRole != "Admin")
                {
                    up.DeleteGran.IsEnabled = false;
                }
                //Grant prop ID & donation ID with expense
                //p.DonationID
                var expenseTotal = (from ex in db.Expenses
                                    where ex.DonationID == p.DonationID
                                    select ex).Count();
                if(expenseTotal > 0) { up.DonAmount.IsEnabled = false; up.AmountRem.IsEnabled = false; }
                up.DonationDate.SelectedDate = p.DonationDate;
                up.DonationExpirationDate.SelectedDate = p.ExpirationDate;
                up.Topmost = true;
                up.Show();
            }
        }
        private void Edit_InKindItem(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1 && StaffDBRole != "Basic")
            {
                DataGrid dg = sender as DataGrid;
                InKindItem p = (InKindItem)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateInKindItem up = new UpdateInKindItem(p);
                if (StaffDBRole != "Admin")
                {
                    up.DeleteItem.IsEnabled = false;
                }
                up.DateRecieved.SelectedDate = p.DateRecieved;
                up.Topmost = true;
                up.Show();
            }
        }
        private void Edit_InKindService(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1 && StaffDBRole != "Basic")
            {
                DataGrid dg = sender as DataGrid;
                InKindService p = (InKindService)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateInKindService up = new UpdateInKindService(p);
                if (StaffDBRole != "Admin")
                {
                    up.DeleteService.IsEnabled = false;
                }
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
        private void Edit_Events(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 1 )
            {
                DataGrid dg = sender as DataGrid;
                EventsDataGrid p = (EventsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateEvent up = new UpdateEvent(p, StaffDBRole);
                if (StaffDBRole != "Admin")
                {
                    up.Delete.IsEnabled = false;
                }
                if(StaffDBRole == "Basic")
                {
                    up.Delete.IsEnabled = false;
                    up.UpEvent.IsEnabled = false;
                    up.AddDonation.IsEnabled = false;
                    up.AddItem.IsEnabled = false;
                    up.AddService.IsEnabled = false;
                }
                if (p.EventStartDateTime.Hour >= 12)
                {
                    up.AMPM_Start.SelectedIndex = 1;
                }
                else
                {
                    up.AMPM_Start.SelectedIndex = 0;
                }
                if (p.EventEndDateTime.Hour >= 12)
                {
                    up.AMPM_End.SelectedIndex = 1;
                }
                else
                {
                    up.AMPM_End.SelectedIndex = 0;
                }
                up.DateRecieved.SelectedDate = p.EventStartDateTime;
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
                ch.Gender.SelectedIndex = 0;
                ch.AgeGroup.SelectedIndex = 0;
                ch.ethnicity.SelectedIndex = 0;
                ch.Show();
            }
        }

        private void Grants_Grid(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel(); 
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
            db = new FCS_DBModel(); 
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
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void InKindItemGrid(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel(); 
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         join d in db.Donations on  p.DonorID equals d.DonorID
                         join ki in db.In_Kind_Item on d.DonationID equals ki.DonationID
                         where (p.DonorType == "Anonymous" || p.DonorType == "Individual")
                         && d.EventID == null
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
                        where (p.DonorType == "Organization" || p.DonorType == "Government")
                         && d.EventID == null
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
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void Events_Grid(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel(); 
            var join1 = (from p in db.FundRaisingEvents
                         select new EventsDataGrid
                         {
                             EventID = p.EventID,
                             EventStartDateTime = p.EventStartDateTime,
                             EventEndDateTime = p.EventEndDateTime,
                             EventName = p.EventName,
                             EventDescription = p.EventDescription
                         });
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();

        }

        //private void Reports_Grid(object sender, RoutedEventArgs e)
        //{
        //    ReportsDataGrid r1 = new ReportsDataGrid("Summer Fund Raiser", "It was great");
        //    ReportsDataGrid r2 = new ReportsDataGrid("Fall Fund Raiser", "It was great");
        //    Reports = new ObservableCollection<ReportsDataGrid>();
        //    Reports.Add(r1);
        //    Reports.Add(r2);
        //    var grid = sender as DataGrid;
        //    grid.ItemsSource = Reports;
        //}

        private void Admin_Grid(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel(); 
            var join1 = (from p in db.Staff
                         select new AdminDataGrid
                         {
                             StaffID = p.StaffID,
                             StaffUserName = p.StaffUserName,
                             StaffFirstName = p.StaffFirstName,
                             StaffLastName = p.StaffLastName,
                             StaffTitle = p.StaffTitle,
                             StaffDBRole = p.StaffDBRole
                         });
            //AdminDataGrid a1 = new AdminDataGrid("13224", "Billy", "Joel");
            //AdminDataGrid a2 = new AdminDataGrid("12347", "Lionnel", "Messi");
            //Admins = new ObservableCollection<AdminDataGrid>();
            //Admins.Add(a1);
            //Admins.Add(a2);
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
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
            db = new FCS_DBModel(); 
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         join d in db.Donations on p.DonorID equals d.DonorID
                         join ki in db.In_Kind_Service on d.DonationID equals ki.DonationID
                         where (p.DonorType == "Anonymous" || p.DonorType == "Individual")
                         && d.EventID == null
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
                ViewGrantProposals vgp = new ViewGrantProposals(StaffDBRole);
                vgp.Show();
            }
            else
            {
                MessageBox.Show("Close other windows");
            }
        }


        private void Add_InKind_Item(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                AddInKindItem iki = new AddInKindItem(false, -1);
                iki.Show();
                iki.Topmost = true;
                iki.Organization.IsEnabled = false;
            }
        }

        private void Add_InKind_Service(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                AddInKindService iks = new AddInKindService(false, -1);
                iks.Show();
                iks.Topmost = true;
                iks.AMPM_End.SelectedIndex = 0;
                iks.AMPM_Start.SelectedIndex = 0;             
            }
        }

        private void CreateNewAccount(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                CreateNewAccount cna = new CreateNewAccount();
                cna.Show();
                cna.UserRole.SelectedIndex = 0;
                cna.Topmost = true;
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

        private void CreateNewEvent(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                CreateNewEvent ne = new CreateNewEvent();
                ne.Show();
                ne.AMPM_End.SelectedIndex = 0;
                ne.AMPM_Start.SelectedIndex = 0;
                ne.Topmost = true;
            }
        }

        private void Donation_Grid(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel(); 
            var join2 = (from p in db.Purposes
                         join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                         join d in db.Donations on dp.DonationID equals d.DonationID
                         join dr in db.Donors on d.DonorID equals dr.DonorID
                         join c in db.DonorContacts on dr.DonorID equals c.DonorID
                         where (dr.DonorType == "Anonymous" || dr.DonorType == "Individual")
                         && d.GrantProposalID == null
                         select new DonationsGrid
                         {
                             DonationAmount = d.DonationAmount,
                             DonationAmountRemaining = d.DonationAmountRemaining,
                             DonationDate = d.DonationDate,
                             PurposeName = p.PurposeName,
                             PurposeDescription = p.PurposeDescription,
                             PurposeID = p.PurposeID,
                             DonationID = d.DonationID,
                             DonorID = dr.DonorID,
                             DonorFirstName = c.ContactFirstName,
                             DonorLastName = c.ContactLastName,
                             OrganizationName = ""
                         }).Union(
                       from p in db.Purposes
                       join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                       join d in db.Donations on dp.DonationID equals d.DonationID
                       join dr in db.Donors on d.DonorID equals dr.DonorID
                       where (dr.DonorType == "Organization" || dr.DonorType == "Government")
                         && d.GrantProposalID == null
                       select new DonationsGrid
                       {
                           DonationAmount = d.DonationAmount,
                           DonationAmountRemaining = d.DonationAmountRemaining,
                           DonationDate = d.DonationDate,
                           PurposeName = p.PurposeName,
                           PurposeDescription = p.PurposeDescription,
                           PurposeID = p.PurposeID,
                           DonationID = d.DonationID,
                           DonorID = dr.DonorID,
                           DonorFirstName = "",
                           DonorLastName = "",
                           OrganizationName = dr.OrganizationName
                       });
            //DonorsDataGrid d1 = new DonorsDataGrid("Tom", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "", "Clinton", "Utah", "84015");
            //DonorsDataGrid d2 = new DonorsDataGrid("Spencer", "Fronberg", "HAFB", "Charity", "1326 North 1590 West", "652 West 800 North", "Clinton", "Utah", "84015");
            //Donors = new ObservableCollection<DonorsDataGrid>();
            //Donors.Add(d1);
            //Donors.Add(d2);
            var grid = sender as DataGrid;
            grid.ItemsSource = join2.ToList();
        }

        private void EditDonation(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count < 2 && StaffDBRole != "Basic")
            {
                DataGrid dg = sender as DataGrid;
                DonationsGrid p = (DonationsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateDonation up = new UpdateDonation(p);
                if (StaffDBRole != "Admin")
                {
                    up.DeleteDon.IsEnabled = false;
                }
                up.DonationDate.SelectedDate = p.DonationDate;
                up.Show();
                this.Topmost = false;
                up.Topmost = true;
            }
        }
        private void Edit_Expense(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (StaffDBRole == "Admin")
            {
                DataGrid dg = sender as DataGrid;
                SessionsGrid p = (SessionsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateSession up = new UpdateSession(p);
                up.Show();
                this.Topmost = false;
                up.Topmost = true;
            }
        }

        private void EditAccount(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count < 2 && StaffDBRole != "Basic")
            {
                DataGrid dg = sender as DataGrid;
                AdminDataGrid p = (AdminDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateAccount up = new UpdateAccount(p);
                if(p.StaffDBRole == "No Access")
                {
                    up.UserRole.SelectedIndex = 0;
                }
                else if(p.StaffDBRole == "Basic")
                {
                    up.UserRole.SelectedIndex = 1;
                }
                else if (p.StaffDBRole == "User")
                {
                    up.UserRole.SelectedIndex = 2;
                }
                else if (p.StaffDBRole == "Admin")
                {
                    up.UserRole.SelectedIndex = 3;
                }
                up.Topmost = true;
                up.Show();
            }
        }

        private void Open_CreateNewSession(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 1)
            {
                AppointmentType at = new AppointmentType();
                at.AMPM_Start.SelectedIndex = 0;
                at.AMPM_End.SelectedIndex = 0;
                at.ApptType.SelectedIndex = 0;
                at.Show();
                //at.Topmost = true;
            }
        }
        private void Sessions_Grid(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel(); 
            var join1 = from s in db.Staff
                        join a in db.Appointments on s.StaffID equals a.StaffID
                        join ex in db.Expenses on a.AppointmentID equals ex.AppointmentID
                        join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
                        join p in db.Patients on ex.PatientID equals p.PatientID
                        select new SessionsGrid
                        {
                            StaffFirstName = s.StaffFirstName,
                            StaffLastName = s.StaffLastName,
                            ClientFirstName = p.PatientFirstName,
                            ClientLastName = p.PatientLastName,
                            AppointmentStart = a.AppointmentStartDate,
                            AppointmentEnd = a.AppointmentEndDate,
                            ExpenseDueDate = ex.ExpenseDueDate,
                            ExpensePaidDate = ex.ExpensePaidDate,
                            DonorBill = ex.DonorBill,
                            PatientBill = ex.PatientBill,
                            TotalExpense = ex.TotalExpenseAmount,
                            ExpenseType = et.ExpenseType1,
                            ExpenseDescription = et.ExpenseDescription,
                            ExpenseID = ex.ExpenseID
                        };
             //... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();


        }

        private void Refresh_Grants(object sender, RoutedEventArgs e)
        {
            sender = Grant_DataGrid;
            Grants_Grid(sender, e);
        }

        private void Refresh_Session(object sender, RoutedEventArgs e)
        {
            sender = Session_DataGrid;
            Sessions_Grid(sender,e);
        }

        private void Refresh_Donor(object sender, RoutedEventArgs e)
        {
            sender = Donor_DataGrid;
            Donor_Grid(sender, e);
        }

        private void Refresh_InKind(object sender, RoutedEventArgs e)
        {
            sender = InKind_DataGrid;
            InKindItemGrid(sender, e);
        }

        private void Refresh_Service(object sender, RoutedEventArgs e)
        {
            sender = Service_DataGrid;
            InKindServiceGrid(sender, e);
        }

        private void Refresh_Events(object sender, RoutedEventArgs e)
        {
            sender = Event_DataGrid;
            Events_Grid(sender, e);
        }
        public void Refresh_Patients(object sender, RoutedEventArgs e)
        {
            ShouldLoadPatient = true;
            ShouldRefreshPatients = true;
            sender = PatientGrid;
            Patient_Grid(sender, e);
        }

        private void Refresh_Admin(object sender, RoutedEventArgs e)
        {
            sender = Admin_DataGrid;
            Admin_Grid(sender, e);
        }

        private void Create_Backup(object sender, RoutedEventArgs e)
        {
            //if (Application.Current.Windows.Count <= 1)
            //{
            //    CreateBackup cb = new CreateBackup();
            //    cb.Show();
            //}
        }
    }
}