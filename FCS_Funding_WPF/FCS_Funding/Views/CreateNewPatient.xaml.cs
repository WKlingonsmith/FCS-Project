using FCS_Funding.Models;
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
    /// Interaction logic for CreateNewPatient.xaml
    /// </summary>
    public partial class CreateNewPatient : Window
    {

        //properties
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int patientOQ { get; set; }
        public Boolean headOfHouse { get; set; }
        public string PatientGender { get; set; }
        public DateTime date { get; set; }
        public int familyOQNumber { get; set; }
        public string relationToHead { get; set; }
        private string ageGroup { get; set; }
        private string ethnicGroup { get; set; }
        
        //helper variables
        private int headOfHousehold { get; set; }
        private int disableTexbox { get; set; }

        public CreateNewPatient()
        {
            headOfHousehold = 0;
            disableTexbox = 0;
            headOfHouse = false;
            InitializeComponent();
            Determine_PatientProblem();
        }

        private void Add_Client(object sender, RoutedEventArgs e)
        {
            
            Determine_AgeGroup(this.AgeGroup.SelectedIndex);
            Determine_EthnicGroup(this.ethnicity.SelectedIndex);
            Determine_Gender(this.Gender.SelectedIndex);
            if (this.firstName != null && this.firstName != "" && this.lastName != null && this.lastName != "" && patientOQ > 0 &&
                 PatientGender != null && PatientGender != "" && this.ageGroup != null && this.ethnicGroup != null
                && this.relationToHead != null && this.relationToHead != "")
            {
                //Need to add another household and open the NewHousehold View
                if(!Family_OQ.IsEnabled)
                {
                    CreateHousehold ch = new CreateHousehold(this.firstName, this.lastName, this.patientOQ, this.PatientGender, this.headOfHouse, this.ageGroup, this.ethnicGroup, this.relationToHead);
                    this.Close();
                    ch.HouseholeIncomeBracket.SelectedIndex = 0;
                    ch.Show();

                }
                //Need to add the client with the family OQ Number
                else if(familyOQNumber > 0)
                {
                    date = DateTime.Now;
                    MessageBox.Show(firstName + "\n" + lastName + "\n" + patientOQ + "\n" + PatientGender + "\n" + headOfHouse + "\n" + ageGroup + "\n" + ethnicGroup + "\n" + 
                        familyOQNumber + "\n" + "\n" + relationToHead + "\n" + date);
                    //this.Close();

                    FCS_FundingDBModel db = new FCS_FundingDBModel();
                    try
                    {
                        int householdID = db.Patients.Where(x => x.PatientOQ == familyOQNumber).Select(x => x.HouseholdID).Distinct().First();
                        Patient pat2 = new Patient();

                        pat2.PatientOQ = patientOQ;
                        pat2.HouseholdID = householdID;
                        pat2.PatientFirstName = firstName;
                        pat2.PatientLastName = lastName;
                        pat2.PatientAgeGroup = ageGroup;
                        pat2.PatientEthnicity = ethnicGroup;
                        pat2.NewClientIntakeHour = date;
                        pat2.IsHead = headOfHouse;
                        pat2.RelationToHead = relationToHead;

                        db.Patients.Add(pat2);
                        db.SaveChanges();
                        MessageBox.Show("Successfully added client.");
                        this.Close();
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("The family member OQ number you entered is invalid.\n Note: Make sure you add a household if your household hasn't been added by clicking the  \"First Member of Household?\" checkbox.");
                    }
                }
                //They are missing the client OQ number.
                else
                {
                    MessageBox.Show("Unable to add client because you are missing the Family Member's Client OQ number that already exists in the system.");
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
            if((headOfHousehold % 2) == 0)
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
            if((disableTexbox % 2) == 0)
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
       
        private void Determine_PatientProblem()
        {
            FCS_FundingDBModel db = new FCS_FundingDBModel();
            List<string> problems = new List<string>();
            foreach (var item in db.Problems)
            {
                problems.Add(item.ProblemType);
            }
            Patient_Problem.ItemsSource = problems.AsEnumerable();
          
            //}
        }

        private void Patient_Problem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
