
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using FCS_DataTesting;
using System.Windows.Controls.Primitives;
using FCS_Funding.Models;
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
        private string StaffDBRole { get; set; }


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
            DetermineProblems();
        }

        private void Update_Patient(object sender, RoutedEventArgs e)
        {
            FCS_FundingDBModel db = new FCS_FundingDBModel();
            int patID = db.Patients.Where(x => x.PatientOQ == pOQ).Select(x => x.PatientID).Distinct().First();


            Determine_AgeGroup(this.AgeGroup.SelectedIndex);
            Determine_EthnicGroup(this.Ethnicity.SelectedIndex);
            Determine_Gender(this.Gender.SelectedIndex);


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
            patient.IsHead = TheHead.IsChecked.Value;
            UpdateProblems();
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
                FCS_FundingDBModel db = new FCS_FundingDBModel();
                int patID = db.Patients.Where(x => x.PatientOQ == pOQ).Select(x => x.PatientID).Distinct().First();
                var patient = (from p in db.Patients
                               where p.PatientID == patID
                               select p).First();
                var patProblems = (from p in db.PatientProblems where p.PatientID == patID select p);
                db.Patients.Remove(patient);
                foreach (var item in patProblems)
                {
                    db.PatientProblems.Remove(item);
                }
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

        private void Patient_Problem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddNewSession(object sender, RoutedEventArgs e)
        {
            AddSession ans = new AddSession(PatientID);
            ans.Show();
            ans.ExpensePaidDate.IsEnabled = false;
            ans.Topmost = true;
        }

        private void SessionsGrid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            var join1 = from s in db.Staffs
                        join a in db.Appointments on s.StaffID equals a.StaffID
                        join ex in db.Expenses on a.AppointmentID equals ex.AppointmentID
                        join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
                        where ex.PatientID == PatientID
                        select new SessionsGrid
                        {
                            StaffFirstName = s.StaffFirstName,
                            StaffLastName = s.StaffLastName,
                            AppointmentStart = a.AppointmentStartDate,
                            AppointmentEnd = a.AppointmentEndDate,
                            ExpenseDueDate = ex.ExpenseDueDate,
                            ExpensePaidDate = ex.ExpensePaidDate,
                            DonorBill = ex.DonorBill,
                            PatientBill = ex.PatientBill,
                            TotalExpense = ex.TotalExpenseAmount,
                            ExpenseType = et.ExpenseType1,
                            ExpenseDescription = et.ExpenseDescription
                        };
            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }
        private void UpdateProblems()
        {
            
            FCS_FundingDBModel db = new FCS_FundingDBModel();
            int patID = db.Patients.Where(x => x.PatientOQ == pOQ).Select(x => x.PatientID).Distinct().First();
            var toggle = PatientProblemsCheckBoxes.Children;
            var patProblems = (from p in db.PatientProblems where p.PatientID == patID select p);
            //CreateNewPatient cnp = new CreateNewPatient();
            PatientProblem patProb = new PatientProblem();
            string checkBoxContent = "";
            int probID = 0;
            var problemTable = db.Problems;
            foreach (var item in toggle)
            {
                checkBoxContent = ((((ContentControl)item).Content).ToString());
                //var distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID select p);
                if (((ToggleButton)item).IsChecked == true)
                {

                    switch (checkBoxContent)
                    {
                        case "Depression":
                            probID = 1;
                            break;
                        case "Bereavement/Loss":
                            probID = 2;
                            break;
                        case "Communication":
                            probID = 3;
                            break;
                        case "Domestic Violence":
                            probID = 4;
                            break;
                        case "Hopelessness":
                            probID = 5;
                            break;
                        case "Work Problems":
                            probID = 6;
                            break;
                        case "Parent Problems":
                            probID = 7;
                            break;
                        case "Substance Abuse":
                            probID = 8;
                            break;
                        case "Problems w/ School":
                            probID = 9;
                            break;
                        case "Marriage/Relationship/Family":
                            probID = 10;
                            break;
                        case "Thoughts of Hurting Self":
                            probID = 11;
                            break;
                        case "Angry Feelings":
                            probID = 12;
                            break;
                        case "Sexual Abuse":
                            probID = 13;
                            break;
                        case "Emotional Abuse":
                            probID = 14;
                            break;
                        case "Physical Abuse":
                            probID = 15;
                            break;
                        case "Problems with the Law":
                            probID = 16;
                            break;
                        case "Unhappy with Life":
                            probID = 17;
                            break;
                        case "Anxiety":
                            probID = 18;
                            break;
                        case "Other":
                            probID = 19;
                            break;
                    }

                    patProb.PatientID = patID;
                    patProb.ProblemID = probID;
                    db.PatientProblems.Add(patProb);
                    db.SaveChanges();
                    patProb = new PatientProblem();
                }
                else
                {
                    var distinctPatProblems = new List<PatientProblem>().AsQueryable();
                    switch (checkBoxContent)
                    {
                        case "Depression":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 1 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {
                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Bereavement/Loss":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 2 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Communication":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 3 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Domestic Violence":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 4 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Hopelessness":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 5 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Work Problems":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 6 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Parent Problems":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 7 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Substance Abuse":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 8 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Problems w/ School":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 9 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Marriage/Relationship/Family":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 10 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Thoughts of Hurting Self":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 11 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Angry Feelings":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 12 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Sexual Abuse":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 13 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Emotional Abuse":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 14 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Physical Abuse":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 15 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Problems with the Law":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 16 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Unhappy with Life":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 17 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Anxiety":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 18 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {

                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                        case "Other":
                            try
                            {
                                distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patID && p.ProblemID == 19 select p);
                                foreach (var thing in distinctPatProblems)
                                {
                                    db.PatientProblems.Remove(thing);
                                }
                                db.SaveChanges();
                            }
                            catch
                            {
                            }
                            distinctPatProblems = new List<PatientProblem>().AsQueryable();
                            break;
                    }
                }
            }
            GC.Collect();
        }
        private void DetermineProblems()
        {
            var toggle = PatientProblemsCheckBoxes.Children;
            List<string> currentProblems = new List<string>();

            FCS_FundingDBModel db = new FCS_FundingDBModel();
            PatientProblem patProb = new PatientProblem();
            int patID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();
            foreach (var item in db.PatientProblems.Where(x => x.PatientID == patID).Select(x => x.ProblemID))
            {
                switch (item)
                {
                    case 1:
                        currentProblems.Add("Depression");
                        break;
                    case 2:
                        currentProblems.Add("Bereavement/Loss");
                        break;
                    case 3:
                        currentProblems.Add("Communication");
                        break;
                    case 4:
                        currentProblems.Add("Domestic Violence");
                        break;
                    case 5:
                        currentProblems.Add("Hopelessness");
                        break;
                    case 6:
                        currentProblems.Add("Work Problems");
                        break;
                    case 7:
                        currentProblems.Add("Parent Problems");
                        break;
                    case 8:
                        currentProblems.Add("Substance Abuse");
                        break;
                    case 9:
                        currentProblems.Add("Problems w/ School");
                        break;
                    case 10:
                        currentProblems.Add("Marriage/Relationship/Family");
                        break;
                    case 11:
                        currentProblems.Add("Thoughts of Hurting Self");
                        break;
                    case 12:
                        currentProblems.Add("Angry Feelings");
                        break;
                    case 13:
                        currentProblems.Add("Sexual Abuse");
                        break;
                    case 14:
                        currentProblems.Add("Emotional Abuse");
                        break;
                    case 15:
                        currentProblems.Add("Physical Abuse");
                        break;
                    case 16:
                        currentProblems.Add("Problems with the Law");
                        break;
                    case 17:
                        currentProblems.Add("Unhappy with Life");
                        break;
                    case 18:
                        currentProblems.Add("Anxiety");
                        break;
                    case 19:
                        currentProblems.Add("Other");
                        break;
                }
            }
            foreach (var item in toggle)
            {
                foreach (var curProb in currentProblems)
                {
                    if ((((ContentControl)item).Content).ToString() == curProb)
                    {
                        ((ToggleButton)item).IsChecked = true;
                    }
                }
            }
            GC.Collect();
        }
    }
}
