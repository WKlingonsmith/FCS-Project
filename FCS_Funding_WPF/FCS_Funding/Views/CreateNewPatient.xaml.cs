using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FCS_Funding.Views
{
	using FCS_Funding;

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
	//	household properties
		private string Income { get; set; }
		public int HouseholdPopulation { get; set; }
		public string County { get; set; }

		private string ageGroup { get; set; }
		private string ethnicGroup { get; set; }

		//helper variables
		private int disableTexbox { get; set; }
		List<ProbCheckBoxModel> problemItems = new List<ProbCheckBoxModel>();

		public CreateNewPatient()
		{
			disableTexbox = 0;
			headOfHouse = false;
			InitializeComponent();
		}

		private void Add_Client(object sender, RoutedEventArgs e)
		{
			Patient tempPatient = new Patient();
			Problem tempProblem = new Problem();
			FCS_DBModel db = new FCS_DBModel();

			
			Determine_AgeGroup(combobox_AgeGroup.SelectedIndex);
			Determine_EthnicGroup(combobox_ethnicity.SelectedIndex);
			Determine_Gender(combobox_Gender.SelectedIndex);
			var togglePatientProblems = PatientProblemsCheckBoxes.Children;

			try
			{
			//	Check to see if there needs to be a new household made first
				if ((bool)check_FirstHouseholdMember.IsChecked)
				{
					Determine_Income(combobox_IncomeBracket.SelectedIndex);
					Determine_County(combobox_County.SelectedIndex);

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
					tempPatient.HouseholdID = db.Patients.Where(x => x.PatientOQ == familyOQNumber).Select(x => x.HouseholdID).Distinct().First();
				}

				bool isHeadOfHouse = (bool)check_HeadOfHousehold.IsChecked;

				tempPatient.PatientOQ = patientOQ;
				tempPatient.PatientFirstName = firstName;
				tempPatient.PatientLastName = lastName;
				tempPatient.PatientAgeGroup = ageGroup;
				tempPatient.PatientEthnicity = ethnicGroup;
				tempPatient.PatientGender = PatientGender;
				tempPatient.NewClientIntakeHour = DateTime.Now;
				tempPatient.IsHead = headOfHouse;
				tempPatient.RelationToHead = (headOfHouse) ? "Head" : relationToHead;
				db.Patients.Add(tempPatient);
				db.SaveChanges();
				Determine_Problems(patientOQ, togglePatientProblems);
						
				this.Close();
			}
			catch (Exception error)
			{
				MessageBox.Show("Something went wrong, please double check your entry values.\n\n");
				MessageBox.Show("Error: " + error.ToString());
			}
		}

		private void Change_HeadOfHousehold(object sender, RoutedEventArgs e)
		{
			CheckForValidInput(sender, null);
			headOfHouse = (bool)check_HeadOfHousehold.IsChecked;
			textbox_RelationToHead.IsEnabled = !(bool)check_HeadOfHousehold.IsChecked;
		}

		private void Change_NewHousehold(object sender, RoutedEventArgs e)
		{
			CheckForValidInput(sender, null);

			bool newHousehold = (bool)check_FirstHouseholdMember.IsChecked;
			textbox_FamilyMemberOQ.IsEnabled = !newHousehold;
			textbox_HouseholdPopulation.IsEnabled = newHousehold;
			combobox_County.IsEnabled = newHousehold;
			combobox_IncomeBracket.IsEnabled = newHousehold;

			//	Initialize the comboboxes
			combobox_County.SelectedIndex = 0;
			combobox_IncomeBracket.SelectedIndex = 0;
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

		private void Determine_County(int selection)
		{
			switch (selection)
			{
				case 0:
					County = "Weber"; break;
				case 1:
					County = "Davis"; break;
				case 2:
					County = "DCLC"; break;
				case 3:
					County = "Morgan"; break;
				case 4:
					County = "Box Elder"; break;
				case 5:
					County = "Other"; break;
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
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void CheckForValidInput(object sender, TextChangedEventArgs e)
		{
			
			try
			{
				if (!string.IsNullOrEmpty(textbox_FirstName.Text) && !string.IsNullOrEmpty(textbox_LastName.Text) && !string.IsNullOrEmpty(textbox_ClientOQ.Text)
					&& (((bool)check_FirstHouseholdMember.IsChecked && !string.IsNullOrEmpty(textbox_HouseholdPopulation.Text))
							 || !string.IsNullOrEmpty(textbox_FamilyMemberOQ.Text)) 
					&& ((bool)check_HeadOfHousehold.IsChecked || !string.IsNullOrEmpty(textbox_RelationToHead.Text)))
				{
					int.Parse(textbox_ClientOQ.Text);
					int.Parse(textbox_HouseholdPopulation.Text);

					AddClient.IsEnabled = true;
					return;
				}
				else
				{
					AddClient.IsEnabled = false;
				}
			} catch {}
		}

		private void txt_NumberOnlyCheck(object sender, TextCompositionEventArgs e)
		{
			CommonControl.NumberOnlyEventCheckNoPeriod(sender, e);
		}

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}

