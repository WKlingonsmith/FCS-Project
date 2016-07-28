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
using FCS_DataTesting;
using FCS_Funding.Views.Windows;

namespace FCS_Funding.Views.Windows
{
	using FCS_Funding;
	using System.Windows.Controls.Primitives;
	using Definition;
	using FCS_DataTesting;
	/// <summary>
	/// Interaction logic for CreateNewPatient.xaml
	/// </summary>
	public class ProbCheckBoxModel
	{
		int PatientID { get; set; }
		int ProblemID { get; set; }
	}

	/// <summary>
	/// Interaction logic for Window_Client.xaml
	/// </summary>
	public partial class Window_Client : Window
	{
		//properties
		public int patientID { get; set; }
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
		List<ProbCheckBoxModel> problemItems = new List<ProbCheckBoxModel>();

		/// <summary>
		/// This creates the dialog to Add or Update a client
		/// </summary>
		/// <param name="bAddClientsOnly"></param>
		public Window_Client(string staffRole, PatientGrid editPatient)
		{
			//	Pre-initialization of variables
			HouseholdPopulation = 1;
			FCS_DBModel db = new FCS_DBModel();

			//	For only adding clients
			if (editPatient == null)
			{
				InitializeComponent();
				check_UpdateHousehold.Visibility = Visibility.Hidden;
				check_ChangeHousehold.Visibility = Visibility.Hidden;
				button_DeleteClient.Visibility = Visibility.Hidden;
				tab_UpdateClient.IsSelected = true;
				tab_UpdateClient.Visibility = Visibility.Collapsed;
				tab_ClientSessions.Visibility = Visibility.Collapsed;

				button_CancelClient.SetValue(Grid.ColumnProperty, 3);

				//	Add Client button stuff
				button_AddUpdateClient.Content = "Add Client";
				button_AddUpdateClient.Click += new RoutedEventHandler(Add_Client);

				textblock_Title.Text = "Add New Client";

				//	Prepopulate the OQ number
				try
				{
					string lastPatientOQ = db.Patients.OrderByDescending(x => x.PatientOQ).Select(x => x.PatientOQ).First();
					int newPatientOQ = int.Parse(lastPatientOQ) + 1;
					textbox_ClientOQ.Text = newPatientOQ.ToString();
					patientOQ = newPatientOQ.ToString();
				}
				catch
				{
					textbox_ClientOQ.Text = "1";
					patientOQ = "1";
				}

				//	Focus for the dialog
				textbox_ClientOQ.Focus();
			}
			//	For updating clients
			else
			{
			//	Get the values from the grid
				firstName = editPatient.FirstName;
				lastName = editPatient.LastName;
				patientOQ = editPatient.PatientOQ;
				relationToHead = editPatient.RelationToHead;
				date = editPatient.Time;
				patientOQ = editPatient.PatientOQ;
				patientID = editPatient.PatientID;

				InitializeComponent();

				//	Hide those UI items that shouldn't exist
				textbox_FamilyMemberOQ.IsEnabled = false;
				textbox_ClientOQ.IsEnabled = false;

				check_FirstHouseholdMember.Visibility = Visibility.Hidden;
			
			//	Manually set the rest of the data to be displayed
				if (editPatient.IsHead)
				{
					check_HeadOfHousehold.IsChecked = editPatient.IsHead;
					textbox_RelationToHead.IsEnabled = false;
					textbox_RelationToHead.Text = "";
				}

				combobox_Gender.Text = editPatient.Gender;
				combobox_AgeGroup.Text = editPatient.AgeGroup;
				combobox_ethnicity.Text = editPatient.Ethnicity;

				//	Get the problems
				ShowProblems();

				//	Get the household information
				int householdID = db.Patients.Where(x => x.PatientID == patientID).Select(x => x.HouseholdID).Distinct().First();
				County = db.PatientHouseholds.Where(x => x.HouseholdID == householdID).Select(x => x.HouseholdCounty).Distinct().First();
				Income = db.PatientHouseholds.Where(x => x.HouseholdID == householdID).Select(x => x.HouseholdIncomeBracket).Distinct().First();
				HouseholdPopulation = db.PatientHouseholds.Where(x => x.HouseholdID == householdID).Select(x => x.HouseholdPopulation).Distinct().First();

				combobox_County.Text = County;
				combobox_IncomeBracket.Text= Income;
				textbox_HouseholdPopulation.Text = HouseholdPopulation.ToString();


				//	Update Client button stuff
				button_AddUpdateClient.Content = "Update Client";
				button_AddUpdateClient.Click += new RoutedEventHandler(Update_Client);

			//	Change the title
				textblock_Title.Text = "Update Client";

				//	Focusing
				textbox_FirstName.Focus();
			}
		}

		private void Add_Client(object sender, RoutedEventArgs e)
		{
			Patient tempPatient = new Patient();
			Problem tempProblem = new Problem();
			FCS_DBModel db = new FCS_DBModel();

			//	Check to see if the OQ number is already taken
			try
			{
				string strPatientOQ = patientOQ.ToString();
				string duplicateQO = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientOQ).Distinct().First();

				if (!string.IsNullOrEmpty(duplicateQO))
				{

					MessageBox.Show("The OQ Number is already taken, please enter a different OQ number.");
					return;
				}
			}
			catch { }

			ageGroup = combobox_AgeGroup.Text;
			ethnicGroup = combobox_ethnicity.Text;
			PatientGender = combobox_Gender.Text;

			try
			{
				//	Check to see if there needs to be a new household made first
				if ((bool)check_FirstHouseholdMember.IsChecked)
				{
					Income = combobox_IncomeBracket.Text;
					County = combobox_County.Text;

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
					try
					{
						tempPatient.HouseholdID = db.Patients.Where(x => x.PatientOQ == familyOQNumber).Select(x => x.HouseholdID).Distinct().First();
					}
					catch (Exception error)
					{
						MessageBox.Show("The provided Family OQ Number does not exist. Please double-check the Family OQ Number.", "Family OQ Number Doesn't Exist", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

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
				Determine_Problems(patientOQ);

                this.Close();
                
            }
			catch (Exception error)
			{
				MessageBox.Show("Something went wrong, please double check your entry values.\n\n");
				MessageBox.Show("Error: " + error.ToString());
			}
            

        }

		private void Update_Client(object sender, RoutedEventArgs e)
		{
			FCS_DBModel db = new FCS_DBModel();
			try
			{
				if ((bool)check_UpdateHousehold.IsChecked)
				{
					int householdID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.HouseholdID).Distinct().First();

					var household = (from h in db.PatientHouseholds
									 where h.HouseholdID == householdID
									 select h).First();

					household.HouseholdCounty = County;
					household.HouseholdIncomeBracket = Income;
					household.HouseholdPopulation = HouseholdPopulation;
					db.SaveChanges();

				}

				int patID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();

				ageGroup = combobox_AgeGroup.Text;
				ethnicGroup = combobox_ethnicity.Text;
				PatientGender = combobox_Gender.Text;


				var patient = (from p in db.Patients
							   where p.PatientID == patID
							   select p).First();


				if ((bool)check_ChangeHousehold.IsChecked)
				{
					string famPatientOQ = textbox_FamilyMemberOQ.Text;

					try
					{
						patient.HouseholdID = db.Patients.Where(x => x.PatientOQ == famPatientOQ).Select(x => x.HouseholdID).Distinct().First();

						if (patient.HouseholdID == 0)
						{
							throw new Exception();
						}
					}
					catch (Exception error)
					{
						MessageBox.Show("The provided Family OQ Number does not exist. Please double-check the Family OQ Number.", "Family OQ Number Doesn't Exist", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
				}
				
				try 
				{

					patient.PatientOQ = patientOQ;
					patient.PatientFirstName = firstName;
					patient.PatientLastName = lastName;
					patient.RelationToHead = relationToHead;
					patient.PatientGender = PatientGender;
					patient.PatientAgeGroup = ageGroup;
					patient.PatientEthnicity = ethnicGroup;
					patient.IsHead = check_HeadOfHousehold.IsChecked.Value;
					UpdateProblems();
					db.SaveChanges();
					this.Close();
				}
				catch (Exception error)
				{
					MessageBox.Show("Error:  " + error.ToString());
				}
			}
			catch
			{
				MessageBox.Show("Please make sure all fields are correct");
			}
		}

		private bool check_ValidFamilyOQNumber(string familyOQ)
		{
			FCS_DBModel db = new FCS_DBModel();
			int householdID = -1;
			householdID = db.Patients.Where(x => x.PatientOQ == familyOQ).Select(x => x.HouseholdID).Distinct().First();

			if (householdID == -1)
				return false;

			return true;

		}

		private void Delete_Client(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Client?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
			if (result == System.Windows.Forms.DialogResult.Yes)
			{
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
				this.Close();
			}
		}
		private void Close_Window(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
        private void Refresh_Sessions(object sender, RoutedEventArgs e)
        {
            sender = grid_Sessions;
            Refresh_SessionsGrid(sender, e);
        }
        private void Refresh_SessionsGrid(object sender, RoutedEventArgs e)
        {
            try
            {
                FCS_DBModel db = new FCS_DBModel();
                int patientID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();
                var join1 = from s in db.Staff
                            join a in db.Appointments on s.StaffID equals a.StaffID
                            join ex in db.Expenses on a.AppointmentID equals ex.AppointmentID
                            join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
                            where ex.PatientID == patientID
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
            catch { }
        }

        private void Change_HeadOfHousehold(object sender, RoutedEventArgs e)
		{
			CheckForValidInput(sender, null);
			headOfHouse = (bool)check_HeadOfHousehold.IsChecked;
			textbox_RelationToHead.IsEnabled = !(bool)check_HeadOfHousehold.IsChecked;
		}

		private void Change_Household(object sender, RoutedEventArgs e)
		{
			CheckForValidInput(sender, null);

			textbox_FamilyMemberOQ.IsEnabled = (bool)check_ChangeHousehold.IsChecked;
		}

		private void Change_UpdateHousehold(object sender, RoutedEventArgs e)
		{
			CheckForValidInput(sender, null);

			bool updateHousehold = (bool)check_UpdateHousehold.IsChecked;
			textbox_HouseholdPopulation.IsEnabled = updateHousehold;
			combobox_County.IsEnabled = updateHousehold;
			combobox_IncomeBracket.IsEnabled = updateHousehold;
		}

		private void Change_NewHousehold(object sender, RoutedEventArgs e)
		{
			CheckForValidInput(sender, null);

			bool newHousehold = (bool)check_FirstHouseholdMember.IsChecked;
			textbox_HouseholdPopulation.IsEnabled = newHousehold;
			combobox_County.IsEnabled = newHousehold;
			combobox_IncomeBracket.IsEnabled = newHousehold;

			if (combobox_IncomeBracket.Text == "")
				combobox_IncomeBracket.SelectedIndex = 0;

			if (combobox_County.Text == "")
				combobox_County.SelectedIndex = 0;

			//	If this is a new house, initialize the comboboxes and such
			textbox_FamilyMemberOQ.IsEnabled = !newHousehold;
		}

		private void AddNewSession(object sender, RoutedEventArgs e)
		{
            DataGrid dg = sender as DataGrid;
            FCS_DBModel db = new FCS_DBModel();
			int patientID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();

			AddSession ans = new AddSession(patientID);
			
            ans.AMPM_Start.SelectedIndex = 0;
			ans.AMPM_End.SelectedIndex = 0;
			ans.ExpensePaidDate.IsEnabled = false;
            ans.ShowDialog();
            Refresh_Sessions(sender, e);
        }

		public void Determine_Problems(string OQ)
		{
			FCS_DBModel db = new FCS_DBModel();
			var toggle = PatientProblemsCheckBoxes.Children;
			var problemTable = db.Problems;

			foreach (var item in toggle)
			{
				string checkboxProblemName = (((ContentControl)item).Content).ToString();
				if (((ToggleButton)item).IsChecked == true)
				{
					try
					{
						PatientProblem patProb = new PatientProblem();
						int patID = db.Patients.Where(x => x.PatientOQ == OQ).Select(x => x.PatientID).Distinct().First();
						patProb.PatientID = patID;
						patProb.ProblemID = GetProblemID(checkboxProblemName);
						db.PatientProblems.Add(patProb);
						db.SaveChanges();

						//var problem = 
					}
					catch (Exception error) { }
				}
			}
		}

		private void UpdateProblems()
		{
			FCS_DBModel db = new FCS_DBModel();

			var toggle = PatientProblemsCheckBoxes.Children;
			var problemTable = db.Problems;

			foreach (var item in toggle)
			{
				string checkboxProblemName = (((ContentControl)item).Content).ToString();
				if (((ToggleButton)item).IsChecked == true)
				{
					PatientProblem patProb = new PatientProblem();
					patProb.PatientID = patientID;
					patProb.ProblemID = GetProblemID(checkboxProblemName);
					db.PatientProblems.Add(patProb);
					db.SaveChanges();
				}
				else
				{
					var distinctPatProblems = new List<PatientProblem>().AsQueryable();
					try
					{
						distinctPatProblems = (from p in db.PatientProblems where p.PatientID == patientID && p.ProblemID == GetProblemID(checkboxProblemName) select p);
						foreach (var thing in distinctPatProblems)
						{
							db.PatientProblems.Remove(thing);
						}
					}
					catch { }

					db.SaveChanges();
				}
			}
			GC.Collect();
		}

		private void ShowProblems()
		{
			var toggle = PatientProblemsCheckBoxes.Children;
			List<int> currentProblems = new List<int>();

			FCS_DBModel db = new FCS_DBModel();
			PatientProblem patProb = new PatientProblem();
			int patID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();
			foreach (var item in db.PatientProblems.Where(x => x.PatientID == patID).Select(x => x.ProblemID))
			{
				currentProblems.Add(item);
			}

			foreach (var item in toggle)
			{
				foreach (var curProb in currentProblems)
				{
					if (curProb == GetProblemID(((ContentControl)item).Content.ToString()))
					{
						((ToggleButton)item).IsChecked = true;
					}
				}
			}
		}

		private int GetProblemID(string strProblemName)
		{
			switch (strProblemName)
			{
				case "Depression":
					return 1;
				case "Bereavement/Loss":
					return 2;
				case "Communication":
					return 3;
				case "Domestic Violence":
					return 4;
				case "Hopelessness":
					return 5;
				case "Work Problems":
					return 6;
				case "Parent Problems":
					return 7;
				case "Substance Abuse":
					return 8;
				case "Problems w/ School":
					return 9;
				case "Marriage/Relationship/Family":
					return 10;
				case "Thoughts of Hurting Self":
					return 11;
				case "Angry Feelings":
					return 12;
				case "Sexual Abuse":
					return 13;
				case "Emotional Abuse":
					return 14;
				case "Physical Abuse":
					return 15;
				case "Problems with the Law":
					return 16;
				case "Unhappy with Life":
					return 17;
				case "Anxiety":
					return 18;
				case "Other":
					return 19;
			}

			return -1;
		}

		private void CheckForValidInput(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (textblock_Title.Text == "Add New Client")
				{
					if (checkForPatientTextEntry() && checkForHeadOfHouseEntry() && checkForHouseholdEntryAddPatient())
					{
						int.Parse(textbox_ClientOQ.Text);
						int.Parse(textbox_HouseholdPopulation.Text);

						if (!(bool)check_FirstHouseholdMember.IsChecked)
						{
							int.Parse(textbox_FamilyMemberOQ.Text);
						}

						button_AddUpdateClient.IsEnabled = true;
						return;
					}
				}
				else
				{
					if (checkForPatientTextEntry() && checkForHeadOfHouseEntry() && checkForChangeHouseholdEntryEditPatient() && checkForUpdateHouseholdEntryEditPatient())
					{
						int.Parse(textbox_ClientOQ.Text);
						int.Parse(textbox_HouseholdPopulation.Text);

						if ((bool)check_ChangeHousehold.IsChecked)
						{
							int.Parse(textbox_FamilyMemberOQ.Text);
						}

						button_AddUpdateClient.IsEnabled = true;
						return;
					}
				}

				button_AddUpdateClient.IsEnabled = false;
			}
			catch (Exception error) { }
		}

		//	Functions for use in CheckForValidInput
			private bool checkForPatientTextEntry()
			{
				return (!string.IsNullOrEmpty(textbox_FirstName.Text) && !string.IsNullOrEmpty(textbox_LastName.Text) 
						&& !string.IsNullOrEmpty(textbox_ClientOQ.Text)) ? true : false;
			}

			private bool checkForHeadOfHouseEntry()
			{
				return ((bool)check_HeadOfHousehold.IsChecked || !string.IsNullOrEmpty(textbox_RelationToHead.Text)) ? true : false;
			}

			private bool checkForHouseholdEntryAddPatient()
			{
					return (((bool)check_FirstHouseholdMember.IsChecked && !string.IsNullOrEmpty(textbox_HouseholdPopulation.Text))
						|| !string.IsNullOrEmpty(textbox_FamilyMemberOQ.Text)) ? true : false;
			}

			private bool checkForChangeHouseholdEntryEditPatient()
			{
				return (!(bool)check_ChangeHousehold.IsChecked || !string.IsNullOrEmpty(textbox_FamilyMemberOQ.Text)) ? true : false;
			}

			private bool checkForUpdateHouseholdEntryEditPatient()
			{
				return (!(bool)check_UpdateHousehold.IsChecked || !string.IsNullOrEmpty(textbox_HouseholdPopulation.Text)) ? true : false;
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
