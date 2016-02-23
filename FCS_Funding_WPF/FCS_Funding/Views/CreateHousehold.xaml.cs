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
        private string Income { get; set; }
        public int HouseholdPopulation { get; set; }
        public string County { get; set; }

        //patient Propeties
        private string firstName { get; set; }
        private string lastName { get; set; }
        private int patientOQ { get; set; }
        private Boolean headOfHouse { get; set; }
        private string gender { get; set; }
        private DateTime date { get; set; }
        private string ageGroup { get; set; }
        private string ethnicGroup { get; set; }
        private string relationToHead { get; set; }

        public CreateHousehold(string fName, string lName, int pOQ, string gen, Boolean head, string aGroup, string ethnicG, string rel)
        {
            firstName = fName;
            lastName = lName;
            patientOQ = pOQ;
            headOfHouse = head;
            gender = gen;
            ageGroup = aGroup;
            ethnicGroup = ethnicG;
            relationToHead = rel;
            InitializeComponent();
        }

        private void Add_Household(object sender, RoutedEventArgs e)
        {
            Determine_Income(this.income.SelectedIndex);
            if (Income != null && HouseholdPopulation > 0 && County != null && County != "")
            {
                date = DateTime.Now;
                MessageBox.Show(firstName + "\n" + lastName + "\n" + patientOQ + "\n" + gender + "\n" + headOfHouse + "\n" + ageGroup + "\n" + ethnicGroup
                    + "\n"  + "\n" + date + "\n" + HouseholdPopulation + "\n" + County + "\n"  + Income);

                FCS_FundingContext db = new FCS_FundingContext();
                PatientHousehold p = new PatientHousehold(HouseholdPopulation, Income, County);
                Patient pat = new Patient(patientOQ, p.HouseholdID, firstName, lastName, gender, ageGroup, ethnicGroup, date, headOfHouse, relationToHead);
                db.PatientHouseholds.Add(p);
                db.Patients.Add(pat);
                db.SaveChanges();
                this.Close();
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you select an income and head of household");                
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
