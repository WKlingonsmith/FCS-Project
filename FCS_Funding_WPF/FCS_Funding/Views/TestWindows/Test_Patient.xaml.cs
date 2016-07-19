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

namespace FCS_Funding.Views.TestWindows
{
	/// <summary>
	/// Interaction logic for Test_Patient.xaml
	/// </summary>
	public partial class Test_Patient : Window
	{
		public Test_Patient()
		{
			InitializeComponent();
		}

		private void button_GetPatient_Click(object sender, RoutedEventArgs e)
		{
			string patientOQ = text_PatientOQ.Text;
			try
			{
				FCS_DBModel db = new FCS_DBModel();
				var patient = (from p in db.Patients
							   where p.PatientOQ == patientOQ
							   select p).First();

				var householdID = (from p in db.Patients
								   where p.PatientOQ == patientOQ
								   select p.HouseholdID).First();

				var household = (from h in db.PatientHouseholds
								 where h.HouseholdID == householdID
								 select h).First();

				text_AgeGroup.Text = patient.PatientAgeGroup;
				text_Ethnicity.Text = patient.PatientEthnicity;
				text_FirstName.Text = patient.PatientFirstName;
				text_Gender.Text = patient.PatientGender;
				text_LastName.Text = patient.PatientLastName;
				text_RelationToHEad.Text = patient.RelationToHead;
				check_IsHead.IsChecked = patient.IsHead;

				text_HouseholdID.Text = household.HouseholdID.ToString();
				text_HouseholdPop.Text = household.HouseholdPopulation.ToString();
				text_Income.Text = household.HouseholdIncomeBracket.ToString();
				text_county.Text = household.HouseholdCounty.ToString();
			}
			catch
			{

			}
		}

		private void button_DeletePatient_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string patientOQ = text_PatientOQ.Text;
				FCS_DBModel db = new FCS_DBModel();
				int patID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();
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
			}
			catch { }
		}

		private void button_AddPatient_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Patient tempPatient = new Patient();
				Problem tempProblem = new Problem();
				FCS_DBModel db = new FCS_DBModel();

				if ((bool)check_NewHousehold.IsChecked)
				{
					int HouseholdPopulation = int.Parse(text_HouseholdPop.Text);
					string Income = text_Income.Text;
					string County = text_county.Text;

					PatientHousehold household = new PatientHousehold();
					household.HouseholdCounty = County;
					household.HouseholdIncomeBracket = Income;
					household.HouseholdPopulation = HouseholdPopulation;
					db.PatientHouseholds.Add(household);
					db.SaveChanges();

					tempPatient.HouseholdID = household.HouseholdID;
				}
				else
				{
					tempPatient.HouseholdID = int.Parse(text_HouseholdID.Text);
				}

				tempPatient.PatientOQ = text_PatientOQ.Text;
				tempPatient.PatientAgeGroup = text_AgeGroup.Text;
				tempPatient.PatientEthnicity = text_Ethnicity.Text;
				tempPatient.PatientFirstName = text_FirstName.Text;
				tempPatient.PatientGender = text_Gender.Text;
				tempPatient.PatientLastName = text_LastName.Text;
				tempPatient.RelationToHead = text_RelationToHEad.Text;
				tempPatient.IsHead = (bool)check_IsHead.IsChecked;
				tempPatient.NewClientIntakeHour = DateTime.Now;

				db.Patients.Add(tempPatient);
				db.SaveChanges();
			}
			catch (Exception error) 
			{ 
				
			}

		}

		private void button_DeleteHousehold_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int householdID = int.Parse(text_HouseholdID.Text);
				FCS_DBModel db = new FCS_DBModel();
				
				var household = (from h in db.PatientHouseholds
								 where h.HouseholdID == householdID
								 select h).First();

				db.PatientHouseholds.Remove(household);
				db.SaveChanges();
			}
			catch { }
		}
	}
}
