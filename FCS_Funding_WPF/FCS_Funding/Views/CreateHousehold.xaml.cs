using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FCS_Funding.Models;
using System.Windows.Controls.Primitives;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateHousehold.xaml
    /// </summary>
    public partial class CreateHousehold : Window
    {
        //private static enum HeadOfHousehold { Mother, Father };
        //Household properies
        private string Income { get; set; }
        public int HouseholdPopulation { get; set; }
        public string County { get; set; }

        //patient Propeties
        private string firstName { get; set; }
        private string lastName { get; set; }
        private string patientOQ { get; set; }
        private Boolean headOfHouse { get; set; }
        private string gender { get; set; }
        private DateTime date { get; set; }
        private string ageGroup { get; set; }
        private string ethnicGroup { get; set; }
        private string relationToHead { get; set; }
        private UIElementCollection togglePatientProblems { get; set; }
        public CreateHousehold(string fName, string lName, string pOQ, string gen, Boolean head, string aGroup, string ethnicG, string rel, UIElementCollection toggle)
        {
            firstName = fName;
            lastName = lName;
            patientOQ = pOQ;
            headOfHouse = head;
            gender = gen;
            ageGroup = aGroup;
            ethnicGroup = ethnicG;
            relationToHead = rel;
            togglePatientProblems = toggle;
            InitializeComponent();
        }

        private void Add_Household(object sender, RoutedEventArgs e)
        {
            Determine_Income(this.HouseholeIncomeBracket.SelectedIndex);
            if (Income != null && HouseholdPopulation > 0 && County != null && County != "")
            {                
                date = DateTime.Now;
                //MessageBox.Show(firstName + "\n" + lastName + "\n" + patientOQ + "\n" + gender + "\n" + headOfHouse + "\n" + ageGroup + "\n" + ethnicGroup
                //    + "\n"  + "\n" + date + "\n" + HouseholdPopulation + "\n" + County + "\n"  + Income);

                FCS_DBModel db = new FCS_DBModel();
                PatientHousehold p = new PatientHousehold();
                p.HouseholdCounty = County;
                p.HouseholdPopulation = HouseholdPopulation;
                p.HouseholdIncomeBracket = Income;

                Patient pat = new Patient();
                pat.PatientOQ = patientOQ;
                pat.HouseholdID = p.HouseholdID;
                pat.PatientFirstName = firstName;
                pat.PatientLastName = lastName;
                pat.PatientGender = gender;
                pat.PatientAgeGroup = ageGroup;
                pat.PatientEthnicity = ethnicGroup;
                pat.NewClientIntakeHour = date;
                pat.IsHead = headOfHouse;
                pat.RelationToHead = relationToHead;                
                db.PatientHouseholds.Add(p);
                db.Patients.Add(pat);
                db.SaveChanges();
                Determine_Problems(patientOQ, togglePatientProblems);
                MessageBox.Show("Successfully added Client and Household.");
                this.Close();
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you select an income, a household population, and input a county.");                
            }
        }
        public void Determine_Problems(string OQ, UIElementCollection toggle)
        {
            PatientProblem patProb = new PatientProblem();
            FCS_DBModel db = new FCS_DBModel();
            string checkBoxContent = "";
            int patID = 0;
            int probID = 0;
            var problemTable = db.Problems;
            foreach (var item in toggle)
            {
                if (((ToggleButton)item).IsChecked == true)
                {
                    patID = db.Patients.Where(x => x.PatientOQ == OQ).Select(x => x.PatientID).Distinct().First();
                    checkBoxContent = ((((ContentControl)item).Content).ToString());
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
            }
        }
        private void Determine_Income(int selection)
        {
            switch (selection)
            {
                case 0:
                    Income = "$0-9,999"; break;
                case 1:
                    Income = "$10,000-14,999"; break;
                case 2:
                    Income = "$15,000-24,000"; break;
                case 3:
                    Income = "$25,000-34,999"; break;
                case 4:
                    Income = "$35,000+"; break;
            }
        }

    }
}
