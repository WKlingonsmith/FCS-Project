using FCS_DataTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AppointmentType.xaml
    /// </summary>
    public partial class AppointmentType : Window
    {
        public string BeginHour { get; set; }
        public string BeginMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public int ExpenseTypeID { get; set; }

        List<PatientGrid> TotalGroup { get; set; }
        List<PatientGrid> Patients { get; set; }
        public bool ShouldRefreshPatients { get; set; }
        public AppointmentType()
        {
            TotalGroup = new List<PatientGrid>();
            Patients = new List<PatientGrid>();
            ShouldRefreshPatients = false;
            InitializeComponent();
        }

        private void Appt_DropDown(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>()
            {
                "Individual",
                "Family",
                "Group"
            };
        }

        private void Select_AppointmentType(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AMPM_Start.SelectedValue.ToString() == "PM" && Convert.ToInt32(BeginHour) != 12)
                {
                    BeginHour = (Convert.ToInt32(BeginHour) + 12).ToString();
                }
                if (AMPM_End.SelectedValue.ToString() == "PM" && Convert.ToInt32(EndHour) != 12)
                {
                    EndHour = (Convert.ToInt32(EndHour) + 12).ToString();
                }
                if (AMPM_Start.SelectedValue.ToString() == "AM" && Convert.ToInt32(BeginHour) == 12)
                {
                    BeginHour = (Convert.ToInt32(BeginHour) - 12).ToString();
                }
                if (AMPM_End.SelectedValue.ToString() == "AM" && Convert.ToInt32(EndHour) == 12)
                {
                    EndHour = (Convert.ToInt32(EndHour) - 12).ToString();
                }

                DateTime help = Convert.ToDateTime(DateRecieved.ToString());
                DateTime startDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(BeginHour), Convert.ToInt32(BeginMinute), 0);
                DateTime endDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(EndHour), Convert.ToInt32(EndMinute), 0);
                DateTime expenseDueDate = startDateTime.AddDays(30);
                string[] separators = new string[] { ", " };
                string staff = Staff.SelectedValue.ToString();
                string cancellationType = "Not Cxl";
                switch (CancellationType.SelectedIndex)
                {
                    case 0:
                        cancellationType = "Not Cxl";
                        break;
                    case 1:
                        cancellationType = "No Show";
                        break;
                    case 2:
                        cancellationType = "Late Cxl";
                        break;                 
                    case 3:
                        cancellationType = "Cxl";
                        break;
                    default:
                        cancellationType = "Not Cxl";
                        break;
                }
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                string[] words = staff.Split(separators, StringSplitOptions.None);
                string FName = words[0]; string LName = words[1]; string usertitle = words[2];  string username = words[3];
                var staffID = (from dc in db.Staff
                               where dc.StaffFirstName == FName && dc.StaffLastName == LName && dc.StaffUserName == username
                               select dc.StaffID).Distinct().FirstOrDefault();
                if (TotalGroup.Count == 0) { MessageBox.Show("Please add at least one client."); return; }
                if (TotalGroup.Count > 1 && (String)ApptType.SelectedItem == "Individual") { MessageBox.Show("Individual appointment may only have one client."); return; }
                //individual (1st option) (ExpenseTypeID = 1 in database)
                //group (3rd option) (ExpenseTypeID = 2 in database)
                if (ApptType.SelectedIndex == 0 || ApptType.SelectedIndex == 2)
                {
                    if (ApptType.SelectedIndex == 0) { ExpenseTypeID = 1; }
                    else { ExpenseTypeID = 2; }
                    Models.Appointment a = new Models.Appointment();
                    a.StaffID = staffID;
                    a.AppointmentStartDate = startDateTime;
                    a.AppointmentEndDate = endDateTime;
                    a.AppointmentCancelationType = cancellationType;
                    db.Appointments.Add(a);
                    db.SaveChanges();

                    foreach (var item in TotalGroup)
                    {
                        AddGroupSession ags = new AddGroupSession(ExpenseTypeID, item, staffID, expenseDueDate, startDateTime, endDateTime, a.AppointmentID);
                        ags.ShowDialog();
                        ags.ExpensePaidDate.IsEnabled = false;
                        ags.FN.IsEnabled = false;
                        ags.LN.IsEnabled = false;
                        ags.OQ.IsEnabled = false;
                        ags.MoneyDonation.IsEnabled = true;
                        ags.Grant.IsEnabled = false;
                    }
                    this.Close();
                }
                //family (2nd option) (ExpenseTypeID = 3 in database)
                else if (ApptType.SelectedIndex == 1)
                {
                    ExpenseTypeID = 3;
                    foreach (var item in TotalGroup)
                    {
                        Models.Appointment a = new Models.Appointment();
                        a.StaffID = staffID;
                        a.AppointmentStartDate = startDateTime;
                        a.AppointmentEndDate = endDateTime;
                        a.AppointmentCancelationType = cancellationType;
                        db.Appointments.Add(a);
                        db.SaveChanges();

                        AddGroupSession ags = new AddGroupSession(ExpenseTypeID, item, staffID, expenseDueDate, startDateTime, endDateTime, a.AppointmentID);
                        ags.Show();
                        ags.ExpensePaidDate.IsEnabled = false;
                        ags.FN.IsEnabled = false;
                        ags.LN.IsEnabled = false;
                        ags.OQ.IsEnabled = false;
                        ags.MoneyDonation.IsEnabled = true;
                        ags.Grant.IsEnabled = false;
                    }
                    this.Close();
                }
                
            }
            catch
            {
                
                if (GroupGrid.Items.Count == 0)
                {
                    MessageBox.Show("Please select atleast one client prior to clicking on Select Appointment Type");
                }
                else if (Staff.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a Therapist prior to clicking Select on Appointment Type");
                }
                else if (DateRecieved.Text.Equals("") || DateRecieved == null)
                {
                    MessageBox.Show("Please select a Date prior to clicking Select Appointment Type");
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please check the fields and try again.");
                }
            }

        }
        private void Patient_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
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

            // ... Assign ItemsSource of DataGrid. 
            var grid = sender as DataGrid;
            Patients.AddRange(join1.ToList());
            PatientGrid.ItemsSource = Patients;

        }

        private void Add_Patient(object sender, RoutedEventArgs e)
        {
            foreach (var item in PatientGrid.SelectedItems)
            {
                TotalGroup.Add((PatientGrid)item);
            }
            GroupGrid.ItemsSource = null;
            GroupGrid.ItemsSource = TotalGroup;

            foreach (var item in PatientGrid.SelectedItems)
            {
                Patients.Remove((PatientGrid)item);
            }
            PatientGrid.ItemsSource = null;
            PatientGrid.ItemsSource = Patients;
        }

        private void Remove_Loaded(object sender, RoutedEventArgs e)
        {
            Remove.Content = "<----";
        }

        private void Remove_Patient(object sender, RoutedEventArgs e)
        {

            foreach (var item in GroupGrid.SelectedItems)
            {
                Patients.Add((PatientGrid)item);
            }
            PatientGrid.ItemsSource = null;
            PatientGrid.ItemsSource = Patients;

            foreach (var item in GroupGrid.SelectedItems)
            {
                TotalGroup.Remove((PatientGrid)item);
            }
            GroupGrid.ItemsSource = null;
            GroupGrid.ItemsSource = TotalGroup;
        }

        private void Staff_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var query = (from o in db.Staff orderby o.StaffTitle descending
                         select o.StaffFirstName + ", " + o.StaffLastName + ", " + o.StaffTitle + ", " + o.StaffUserName).ToList();
            // Original line                         select o.StaffFirstName + ", " + o.StaffLastName + ", " + o.StaffUserName).ToList();


            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void AM_PM_Dropdown(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>() { "AM", "PM" };
        }

        private void Hour_LostFocus(object sender, RoutedEventArgs e)
        {

            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (value > 12)
                        textbox.Text = "12";
                    else if (value < 1)
                        textbox.Text = "1";
                }
                catch
                {
                    textbox.Text = "";
                    MessageBox.Show("Please enter a number.");
                }
            }
            else
            {
                textbox.Text = "12";
            }

        }

        private void Minute_LostFocus(object sender, RoutedEventArgs e)
        {
            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (textbox.Text.Length == 1)
                    {
                        textbox.Text = "0" + textbox.Text;
                    }
                    else if (value > 59)
                    {
                        textbox.Text = "59";
                    }
                    else if (value < 0)
                    {
                        textbox.Text = "00";
                    }
                }
                catch
                {
                    textbox.Text = "";
                    MessageBox.Show("Please enter a number.");
                }
            }
            else
            {
                textbox.Text = "00";
            }
        }

        private void Hour_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.Text = "12";
        }

        private void Minute_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.Text = "00";
        }
    }
}
