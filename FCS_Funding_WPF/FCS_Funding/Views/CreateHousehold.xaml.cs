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
using FCS_Funding.Models;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateHousehold.xaml
    /// </summary>
    public partial class CreateHousehold : Window
    {
        //private static enum HeadOfHousehold { Mother, Father };
        //Household properies
        private string HeadOfHousehold { get; set; }
        private string Income { get; set; }
        public int HouseholdPopulation { get; set; }
        public string County { get; set; }

        //patient Propeties
        private string firstName { get; set; }
        private string lastName { get; set; }
        private int patientOQ { get; set; }
        private string notes { get; set; }
        private Boolean headOfHouse { get; set; }
        private string gender { get; set; }
        private DateTime date { get; set; }
        private string ageGroup { get; set; }
        private string ethnicGroup { get; set; }

        public CreateHousehold(string fName, string lName, int pOQ, string gen, Boolean head, string aGroup, string ethnicG, string note)
        {
            firstName = fName;
            lastName = lName;
            patientOQ = pOQ;
            notes = note;
            headOfHouse = head;
            gender = gen;
            ageGroup = aGroup;
            ethnicGroup = ethnicG;
            InitializeComponent();
        }

        private void Add_Household(object sender, RoutedEventArgs e)
        {
            Determine_HeadOfHousehold(this.headOfHousehold.SelectedIndex);
            Determine_Income(this.income.SelectedIndex);
            MessageBox.Show(this.County);
            if (HeadOfHousehold != null && Income != null && HouseholdPopulation > 0 && County != null && County != "")
            {
                date = DateTime.Now;
                MessageBox.Show(firstName + "\n" + lastName + "\n" + patientOQ + "\n" + gender + "\n" + headOfHouse + "\n" + ageGroup + "\n" + ethnicGroup
                    + "\n" + notes + "\n" + date + "\n" + HouseholdPopulation + "\n" + County + "\n" + HeadOfHousehold + "\n" + Income);
                //FCS_DB n = new FCS_DB();
                //n.PatientHousehold.Add()
                //PatientHouseholdMap ph = new PatientHouseholdMap();
                //DataSet1 mydb = new DataSet1();                
                //mydb.PatientHousehold.AddPatientHouseholdRow(HouseholdPopulation, Income, County);
                //mydb.Patient.AddPatientRow(patientOQ, mydb.PatientHousehold.OrderByDescending(u => u.HouseholdID).FirstOrDefault(), firstName, lastName, gender, ageGroup, ethnicGroup, DateTime.Now, headOfHouse);
                //mydb.AcceptChanges();
                //Models.PatientHousehold ph = new Models.PatientHousehold();
                //ph.HouseholdCounty = County;
                //ph.HouseholdIncomeBracket = Income;
                //ph.HouseholdPopulation = HouseholdPopulation;
                FCS_FundingContext db = new FCS_FundingContext();
                PatientHousehold p = new PatientHousehold(HouseholdPopulation, Income, County);
                Patient pat = new Patient(patientOQ, p.HouseholdID, firstName, lastName, gender, ageGroup, ethnicGroup, date, headOfHouse, "Step Child");
                
                db.PatientHouseholds.Add(p);
                db.Patients.Add(pat);
                db.SaveChanges();
                
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you select an income and head of household");                
            }
        }
        private void Determine_HeadOfHousehold(int selection)
        {
            switch (selection)
            {
                case 0:
                    HeadOfHousehold = "Mother"; break;
                case 1:
                    HeadOfHousehold = "Father"; break;
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
