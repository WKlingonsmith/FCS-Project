using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateNewPatient.xaml
    /// </summary>
    public class ProbCheckBoxModel
    {
        int PatientID { get; set; }
        int ProblemID { get; set; }
    }
    public partial class CreateNewPatient : Window
    {

        //properties
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patientOQ { get; set; }
        public Boolean headOfHouse { get; set; }
        public string PatientGender { get; set; }
        public DateTime date { get; set; }
        public string familyOQNumber { get; set; }
        public string relationToHead { get; set; }
        private string ageGroup { get; set; }
        private string ethnicGroup { get; set; }

        //helper variables
        private int headOfHousehold { get; set; }
        private int disableTexbox { get; set; }
        List<ProbCheckBoxModel> problemItems = new List<ProbCheckBoxModel>();
        public CreateNewPatient()
        {
            headOfHousehold = 0;
            disableTexbox = 0;
            headOfHouse = false;
            InitializeComponent();
            //  PatientProblemsGroup();
        }

        private void Add_Client(object sender, RoutedEventArgs e)
        {
            Patient pat2 = new Patient();
            Problem prob = new Problem();
            FCS_DBModel db = new FCS_DBModel();
            Determine_AgeGroup(this.AgeGroup.SelectedIndex);
            Determine_EthnicGroup(this.ethnicity.SelectedIndex);
            var togglePatientProblems = PatientProblemsCheckBoxes.Children;
            Determine_Gender(this.Gender.SelectedIndex);
            if (this.firstName != null && this.firstName != "" && this.lastName != null && this.lastName != "" && patientOQ != null && patientOQ != "" &&
                 PatientGender != null && PatientGender != "" && this.ageGroup != null && this.ethnicGroup != null
                && this.relationToHead != null && this.relationToHead != "")
            {
                //Need to add another household and open the NewHousehold View
                if (!Family_OQ.IsEnabled)
                {
                    CreateHousehold ch = new CreateHousehold(this.firstName, this.lastName, this.patientOQ, this.PatientGender, this.headOfHouse, this.ageGroup, this.ethnicGroup, this.relationToHead, togglePatientProblems);
                    this.Close();
                    ch.HouseholeIncomeBracket.SelectedIndex = 0;
                    ch.Show();
                }
                //Need to add the client with the family OQ Number
                else if (familyOQNumber != null && familyOQNumber != "")
                {
                    date = DateTime.Now;
                    //MessageBox.Show(firstName + "\n" + lastName + "\n" + patientOQ + "\n" + PatientGender + "\n" + headOfHouse + "\n" + ageGroup + "\n" + ethnicGroup + "\n" +
                    //    familyOQNumber + "\n" + "\n" + relationToHead + "\n" + date);
                    //this.Close();                    
                    try
                    {
                    int householdID = db.Patients.Where(x => x.PatientOQ == familyOQNumber).Select(x => x.HouseholdID).Distinct().First();

                    pat2.PatientOQ = patientOQ;
                    pat2.HouseholdID = householdID;
                    pat2.PatientFirstName = firstName;
                    pat2.PatientLastName = lastName;
                    pat2.PatientAgeGroup = ageGroup;
                    pat2.PatientEthnicity = ethnicGroup;
                    pat2.PatientGender = PatientGender;
                    pat2.NewClientIntakeHour = date;
                    pat2.IsHead = headOfHouse;
                    pat2.RelationToHead = relationToHead;                    
                    db.Patients.Add(pat2);
                    db.SaveChanges();
                    Determine_Problems(patientOQ, togglePatientProblems);

                    MessageBox.Show("Successfully added client.");
                    this.Close();

                    }
                    catch 
                    {
                         MessageBox.Show("The OQ number entered is invalid.");
                    }
                }
                //They are missing the client OQ number.
                else
                {
                    MessageBox.Show("This OQ number has already been taken.");
                }
            }
            else
            {
                MessageBox.Show("Unable to add client");
            }

        }

        private void Change_HeadOfHousehold(object sender, RoutedEventArgs e)
        {
            headOfHousehold++;
            if ((headOfHousehold % 2) == 0)
            {
                headOfHouse = false;
            }
            else
            {
                headOfHouse = true;
            }

        }

        private void Disable_Textbox(object sender, RoutedEventArgs e)
        {
            disableTexbox++;
            if ((disableTexbox % 2) == 0)
            {
                Family_OQ.IsEnabled = true;
                Family_OQ.Background = Brushes.White;
            }
            else
            {
                Family_OQ.IsEnabled = false;
                Family_OQ.Background = Brushes.LightGray;
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

        public void Determine_Problems(string OQ, UIElementCollection toggle)
        {
            PatientProblem patProb = new PatientProblem();
            FCS_DBModel db = new FCS_DBModel();
            string checkBoxContent = "";
            int probID = 0;
            var problemTable = db.Problems;
            foreach (var item in toggle)
            {
                if (((ToggleButton)item).IsChecked == true)
                {
                    int patID = db.Patients.Where(x => x.PatientOQ == OQ).Select(x => x.PatientID).Distinct().First();
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
    }
}

