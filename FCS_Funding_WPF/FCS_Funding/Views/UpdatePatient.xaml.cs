
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
using FCS_DataTesting;
namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdatePatient.xaml
    /// </summary>
    public partial class UpdatePatient : Window
    {
        public int patientOQ { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string relationToHead { get; set; }
        public string gender { get; set; }
        public int PatientID { get; set; }

        //setter values
        private string ageGroup { get; set; }
        private string PatientGender { get; set; }
        private string ethnicGroup { get; set; }

        //Helper Variables
        private int headOfHousehold { get; set; }
        private DateTime date { get; set; }
        private int pOQ { get; set; }

        public UpdatePatient(PatientGrid p)
        {
            //var check = sender as CheckBox;
            //check.IsChecked = p.IsHead;
            //check.SetCurrentValue(CheckBox.IsCheckedProperty, p.IsHead);
            //TheHead.SetCurrentValue
            firstName = p.FirstName;
            lastName = p.LastName;
            patientOQ = p.PatientOQ;
            relationToHead = p.RelationToHead;
            date = p.Time;
            pOQ = p.PatientOQ;
            PatientID = p.PatientID;
            InitializeComponent();
        }

        private void Update_Patient(object sender, RoutedEventArgs e)
        {
            Determine_AgeGroup(this.AgeGroup.SelectedIndex);
            Determine_EthnicGroup(this.Ethnicity.SelectedIndex);
            Determine_Gender(this.Gender.SelectedIndex);

            FCS_Funding.Models.FCS_FundingDBModel db = new FCS_Funding.Models.FCS_FundingDBModel();
            int patID = db.Patients.Where(x => x.PatientOQ == pOQ).Select(x => x.PatientID).Distinct().First();
            var patient = (from p in db.Patients
                           where p.PatientID == patID
                          select p).First();
            patient.PatientOQ = patientOQ;
            patient.PatientFirstName = firstName;
            patient.PatientLastName = lastName;
            patient.RelationToHead = relationToHead;
            patient.PatientGender = PatientGender;
            patient.PatientAgeGroup = ageGroup;
            patient.PatientEthnicity = ethnicGroup;
            patient.IsHead =  TheHead.IsChecked.Value;
            db.SaveChanges();
            MessageBox.Show("Successfully Updated Patient");
            this.Close();
            //int householdID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.HouseholdID).Distinct().First();
            //FCS_Funding.Models.Patient update = new FCS_Funding.Models.Patient(patientOQ, householdID, firstName, lastName, PatientGender,
            //    ageGroup, ethnicGroup, date, TheHead.IsChecked.Value, relationToHead);
            //db.Patients.Attach(update);
            //var entry = db.Entry(update);
        }

        private void Detete_Patient(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation", "Are you sure that you want to delete this Patient?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingDBModel db = new FCS_Funding.Models.FCS_FundingDBModel();
                int patID = db.Patients.Where(x => x.PatientOQ == pOQ).Select(x => x.PatientID).Distinct().First();
                var patient = (from p in db.Patients
                               where p.PatientID == patID
                               select p).First();
                db.Patients.Remove(patient);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Patient.");
                this.Close();
            }
        }
        private void Determine_Gender(int selection)
        {
            switch (selection)
            {
                case 0:
                    PatientGender = "Male"; break;
                case 1:
                    PatientGender = "Female"; break;
                case 2:
                    PatientGender = "Other"; break;
            }
        }
        private void Determine_AgeGroup(int selection)
        {
            switch (selection)
            {
                case 0:
                    ageGroup = "0-5"; break;
                case 1:
                    ageGroup = "6-11"; break;
                case 2:
                    ageGroup = "12-17"; break;
                case 3:
                    ageGroup = "18-23"; break;
                case 4:
                    ageGroup = "24-44"; break;
                case 5:
                    ageGroup = "45-54"; break;
                case 6:
                    ageGroup = "55-69"; break;
                case 7:
                    ageGroup = "70+"; break;
            }
        }

        private void Determine_EthnicGroup(int selection)
        {
            switch (selection)
            {
                case 0:
                    ethnicGroup = "African American"; break;
                case 1:
                    ethnicGroup = "Native/Alaskan"; break;
                case 2:
                    ethnicGroup = "Pacific Islander"; break;
                case 3:
                    ethnicGroup = "Asian"; break;
                case 4:
                    ethnicGroup = "Caucasian"; break;
                case 5:
                    ethnicGroup = "Hispanic"; break;
                case 6:
                    ethnicGroup = "Other"; break;
            }
        }

        private void AddNewSession(object sender, RoutedEventArgs e)
        {            
            AddSession ans = new AddSession(PatientID);
            ans.Show();
            ans.Topmost = true;
        }

        private void SessionsGrid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            var join1 = from s in db.Staffs
                        join a in db.Appointments on s.StaffID equals a.StaffID
                        join ex in db.Expenses on a.AppointmentID equals ex.AppointmentID
                        where ex.PatientID == PatientID
                        select new SessionsGrid
                        {
                            StaffFirstName = s.StaffFirstName,
                            StaffLastName = s.StaffLastName,
                            AppointmentStart = a.AppointmentStartDate,
                            AppointmentEnd = a.AppointmentEndDate,
                            ExpenseDueDate = ex.ExpenseDueDate,
                            DonorBill = ex.DonorBill,
                            PatientBill = ex.PatientBill,
                            TotalExpense = ex.TotalExpenseAmount
                        };
            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }
    }
}
