using FCS_DataTesting;
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
using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AppointmentType.xaml
    /// </summary>
    public partial class AppointmentType : Window
    {
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
                "Group/Individual",    
                "Family"
            };
        }

        private void Select_AppointmentType(object sender, RoutedEventArgs e)
        {
            //indiv/group
            if(ApptType.SelectedIndex == 0)
            {
                string[] separators = new string[] { ", " };
                string staff = Staff.SelectedValue.ToString();
                Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
                string[] words = staff.Split(separators, StringSplitOptions.None);
                string FName = words[0]; string LName = words[1]; string username = words[2];
                var staffID = (from dc in db.Staffs
                               where dc.StaffFirstName == FName && dc.StaffLastName == LName && dc.StaffUserName == username
                               select dc.StaffID).Distinct().FirstOrDefault();
            }
            //family
            else if(ApptType.SelectedIndex == 1)
            {

            }
        }
        private void Patient_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
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
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            var query = (from o in db.Staffs
                         select o.StaffFirstName + ", " + o.StaffLastName + ", " + o.StaffUserName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }
    }
}
