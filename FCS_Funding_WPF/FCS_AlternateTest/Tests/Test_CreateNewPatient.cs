using System;
using FCS_Funding;
using FCS_Funding.Views.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnitTestUtilities;
using System.Windows.Controls;
using FCS_Funding.Models;
using FCS_Funding.Views.TestWindows;


namespace FCS_AlternateTest
{
	using Definition;
	using System.Data;
	using System.Data.OleDb;
	using System.Data.SqlClient;
	using System.Linq;
	[TestClass]
	public class UnitTest1
	{
	
		[TestMethod]
		public void TestAddingDuplicatePatient()
		{
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "123451";
			tempPatient.PatientFirstName = "Test";
			tempPatient.PatientLastName = "McGee";
			tempPatient.RelationToHead = "Related";
			tempPatient.PatientGender = "Female";
			tempPatient.PatientEthnicity = "Pacific Islander";
			tempPatient.PatientAgeGroup = "12-17";
			tempPatient.IsHead = false;

			//	Premade household
			PatientHousehold tempHousehold = new PatientHousehold();
			tempHousehold.HouseholdCounty = "Box Elder";
			tempHousehold.HouseholdIncomeBracket = "$25,000-34,999";
			tempHousehold.HouseholdPopulation = 7;

			//	Add the patient with new household
			Test_Patient tpat = OpenTestPatient();
			DeletePatient(tpat, tempPatient.PatientOQ);
			AddPatient(tpat, tempPatient, tempHousehold);

			//	Open the client window
			Window_Client window = OpenCreateNewPatient();

			//	Add the patient
			UIUtilities.ClickOnItem(window.check_HeadOfHousehold);

			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);

			UIUtilities.SelectComboboxItem(window.combobox_AgeGroup, tempPatient.PatientAgeGroup);
			UIUtilities.SelectComboboxItem(window.combobox_ethnicity, tempPatient.PatientEthnicity);
			UIUtilities.SelectComboboxItem(window.combobox_Gender, tempPatient.PatientGender);

			//	Check that it added
			UIUtilities.ClickOnItemNoWait(window.button_AddUpdateClient);

			UIUtilities.CloseWindow(tpat);

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				//	Is the window still open after clicking, because the error dialog should be showing
				//	Test patient is also open, hence the 2 window count
				Assert.AreEqual(2, Application.Current.Windows.Count);
			}));

			UIUtilities.CloseWindow(window);

		}

		[TestMethod]
		public void TestAddingNewHousehold()
		{
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "123451";
			tempPatient.PatientFirstName = "Test";
			tempPatient.PatientLastName = "McGee";
			tempPatient.RelationToHead = "Related";
			tempPatient.PatientGender = "Female";
			tempPatient.PatientEthnicity = "Pacific Islander";
			tempPatient.PatientAgeGroup = "12-17";
			tempPatient.IsHead = false;

			//	Premade household
			PatientHousehold tempHousehold = new PatientHousehold();
			tempHousehold.HouseholdCounty = "Box Elder";
			tempHousehold.HouseholdIncomeBracket = "$25,000-34,999";
			tempHousehold.HouseholdPopulation = 7;

			//	Add the patient with new household
			Test_Patient tpat = OpenTestPatient();
			DeletePatient(tpat, tempPatient.PatientOQ);

			//	Open the client window
			Window_Client window = OpenCreateNewPatient();

			//	Add the patient
			UIUtilities.ClickOnItem(window.check_HeadOfHousehold);

			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);

			UIUtilities.SelectComboboxItem(window.combobox_AgeGroup, tempPatient.PatientAgeGroup);
			UIUtilities.SelectComboboxItem(window.combobox_ethnicity, tempPatient.PatientEthnicity);
			UIUtilities.SelectComboboxItem(window.combobox_Gender, tempPatient.PatientGender);

			//	Add the household
			UIUtilities.ClickOnItem(window.check_FirstHouseholdMember);

			UIUtilities.TypeIntoTextbox(window.textbox_HouseholdPopulation, tempHousehold.HouseholdPopulation.ToString());
			UIUtilities.SelectComboboxItem(window.combobox_County, tempHousehold.HouseholdCounty);
			UIUtilities.SelectComboboxItem(window.combobox_IncomeBracket, tempHousehold.HouseholdIncomeBracket);

			//	Check that it added
			UIUtilities.ClickOnItemNoWait(window.button_AddUpdateClient);

			//	Find the patient
			FindPatient(tpat, tempPatient.PatientOQ);

			//	Check the values
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual(tempHousehold.HouseholdCounty, tpat.text_county.Text);
				Assert.AreEqual(tempHousehold.HouseholdIncomeBracket, tpat.text_Income.Text);
				Assert.AreEqual(tempHousehold.HouseholdPopulation, int.Parse(tpat.text_HouseholdPop.Text));
			}));

			UIUtilities.CloseWindow(tpat);
		}

		[TestMethod]
		public void TestUsingWrongFamilyOQForHousehold()
		{

			//	Patient to test with
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "123451";
			tempPatient.PatientFirstName = "Test";
			tempPatient.PatientLastName = "McGee";
			tempPatient.RelationToHead = "Related";
			tempPatient.PatientGender = "Female";
			tempPatient.PatientEthnicity = "Pacific Islander";
			tempPatient.PatientAgeGroup = "12-17";
			tempPatient.IsHead = false;

			//	Add the temp patient manually, but link to previously-made family member
			Window_Client window = OpenCreateNewPatient();

			UIUtilities.TypeIntoTextbox(window.textbox_FamilyMemberOQ, "01234567");

			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);
			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, tempPatient.RelationToHead);

			UIUtilities.SelectComboboxItem(window.combobox_AgeGroup, tempPatient.PatientAgeGroup);
			UIUtilities.SelectComboboxItem(window.combobox_ethnicity, tempPatient.PatientEthnicity);
			UIUtilities.SelectComboboxItem(window.combobox_Gender, tempPatient.PatientGender);

			UIUtilities.ClickOnItem(window.button_AddUpdateClient);
			
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
			//	Is the window still open after clicking, because the error dialog should be showing
				Assert.AreEqual(1, Application.Current.Windows.Count);
			}));

			UIUtilities.CloseWindow(window);
		}

		[TestMethod]
		public void TestUsingFamilyOQForHousehold()
		{
			//	Premade Family Member
			Patient familyPatient = new Patient();
			familyPatient.PatientOQ = "123450";
			familyPatient.PatientFirstName = "Doodly";
			familyPatient.PatientLastName = "Doo";
			familyPatient.RelationToHead = "DA FATHER";
			familyPatient.PatientGender = "Male";
			familyPatient.PatientEthnicity = "Caucasian";
			familyPatient.PatientAgeGroup = "24-44";
			familyPatient.IsHead = true;

			//	Premade household
			PatientHousehold familyHousehold = new PatientHousehold();
			familyHousehold.HouseholdCounty = "Box Elder";
			familyHousehold.HouseholdIncomeBracket = "$25,000-34,999";
			familyHousehold.HouseholdPopulation = 7;

			//	Patient to test with
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "123451";
			tempPatient.PatientFirstName = "Test";
			tempPatient.PatientLastName = "McGee";
			tempPatient.RelationToHead = "Related";
			tempPatient.PatientGender = "Female";
			tempPatient.PatientEthnicity = "Pacific Islander";
			tempPatient.PatientAgeGroup = "12-17";
			tempPatient.IsHead = false;

			//	Add the patient
			Test_Patient tpat = OpenTestPatient();
			DeletePatient(tpat, tempPatient.PatientOQ);
			DeletePatient(tpat, familyPatient.PatientOQ);
			AddPatient(tpat, familyPatient, familyHousehold);

			//	Add the temp patient manually, but link to previously-made family member
			Window_Client window = OpenCreateNewPatient();

			UIUtilities.TypeIntoTextbox(window.textbox_FamilyMemberOQ, familyPatient.PatientOQ);

			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);
			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, tempPatient.RelationToHead);

			UIUtilities.SelectComboboxItem(window.combobox_AgeGroup, tempPatient.PatientAgeGroup);
			UIUtilities.SelectComboboxItem(window.combobox_ethnicity, tempPatient.PatientEthnicity);
			UIUtilities.SelectComboboxItem(window.combobox_Gender, tempPatient.PatientGender);

			UIUtilities.ClickOnItemNoWait(window.button_AddUpdateClient);

			//	Find the added patient
			FindPatient(tpat, tempPatient.PatientOQ);


			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual(familyHousehold.HouseholdCounty, tpat.text_county.Text);
				Assert.AreEqual(familyHousehold.HouseholdIncomeBracket, tpat.text_Income.Text);
				Assert.AreEqual(familyHousehold.HouseholdPopulation, int.Parse(tpat.text_HouseholdPop.Text));
			}));

			//	Clean up
			DeletePatient(tpat, tempPatient.PatientOQ);
			DeleteHousehold(tpat, familyPatient.PatientOQ);
			DeletePatient(tpat, familyPatient.PatientOQ);

		}

		[TestMethod]
		public void TestAddingPatient()
		{
			//	Set the testing data
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "123451";
			tempPatient.PatientFirstName = "Test";
			tempPatient.PatientLastName = "McGee";
			tempPatient.RelationToHead = "Related";
			tempPatient.PatientGender = "Female";
			tempPatient.PatientEthnicity = "Pacific Islander";
			tempPatient.PatientAgeGroup = "12-17";
			tempPatient.IsHead = false;

			//	TODO: Need to check the patient problems


			Test_Patient tpat = OpenTestPatient();
			DeletePatient(tpat, tempPatient.PatientOQ);

			Window_Client window = OpenCreateNewPatient();

			UIUtilities.ClickOnItem(window.check_FirstHouseholdMember);

			//	Input the data for the comboboxes
			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);
			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, tempPatient.RelationToHead);

			UIUtilities.SelectComboboxItem(window.combobox_AgeGroup, tempPatient.PatientAgeGroup);
			UIUtilities.SelectComboboxItem(window.combobox_ethnicity, tempPatient.PatientEthnicity);
			UIUtilities.SelectComboboxItem(window.combobox_Gender, tempPatient.PatientGender);

			UIUtilities.ClickOnItemNoWait(window.button_AddUpdateClient);

			//	Find the added patient
			FindPatient(tpat, tempPatient.PatientOQ);

			//	Add new test patient
			Patient newPatient = new Patient();

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				newPatient.PatientOQ = tpat.text_PatientOQ.Text;
				newPatient.PatientLastName = tpat.text_LastName.Text;
				newPatient.PatientFirstName = tpat.text_FirstName.Text;
				newPatient.RelationToHead = tpat.text_RelationToHEad.Text;
				newPatient.PatientEthnicity = tpat.text_Ethnicity.Text;
				newPatient.PatientAgeGroup = tpat.text_AgeGroup.Text;
				newPatient.PatientGender = tpat.text_Gender.Text;
			}));

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual(tempPatient.PatientOQ, newPatient.PatientOQ);
				Assert.AreEqual(tempPatient.PatientFirstName, newPatient.PatientFirstName);
				Assert.AreEqual(tempPatient.PatientLastName, newPatient.PatientLastName);
				Assert.AreEqual(tempPatient.RelationToHead, newPatient.RelationToHead);
				Assert.AreEqual(tempPatient.IsHead, newPatient.IsHead);
				Assert.AreEqual(tempPatient.PatientEthnicity, newPatient.PatientEthnicity);
				Assert.AreEqual(tempPatient.PatientAgeGroup, newPatient.PatientAgeGroup);
				Assert.AreEqual(tempPatient.PatientGender, newPatient.PatientGender);
			}));

			DeletePatient(tpat, tempPatient.PatientOQ);

			UIUtilities.CloseWindow(tpat);
		}

	/// <summary>
	/// Tests that the number-only textboxes can only hold numbers
	/// </summary>
		[TestMethod]
		public void TestNumberOnlyInput()
		{
			Window_Client window = OpenCreateNewPatient();

			//	Client OQ Textbo
			UIUtilities.TypeIntoTextboxNoWait(window.textbox_ClientOQ, "123456789abcdefghijklmnopqrstuvwxyz");
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_ClientOQ.Text);
			}));
			
			//	Family OQ Textbox
			UIUtilities.TypeIntoTextboxNoWait(window.textbox_FamilyMemberOQ, "123456789abcdefghijklmnopqrstuvwxyz");
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_FamilyMemberOQ.Text);
			}));

			//	Household Pop
			UIUtilities.ClickOnItem(window.check_FirstHouseholdMember);

			UIUtilities.TypeIntoTextboxNoWait(window.textbox_HouseholdPopulation, "123456789abcdefghijklmnopqrstuvwxyz");
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_HouseholdPopulation.Text);
			}));

			UIUtilities.CloseWindow(window);
		}

		[TestMethod]
		public void TestButtonDisable()
		{
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "1234567890";
			tempPatient.PatientFirstName = "Test";
			tempPatient.PatientLastName = "McGee";
			tempPatient.RelationToHead = "Related";
			tempPatient.PatientGender = "Female";
			tempPatient.PatientEthnicity = "Pacific Islander";
			tempPatient.PatientAgeGroup = "12-17";
			tempPatient.IsHead = false;

			Window_Client window = OpenCreateNewPatient();

			//	Check that the button is initially disabled
			CheckAddPatientButtonState(window, false);

			//	Type into all other entry points, so the button is enabled
			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);
			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, tempPatient.RelationToHead);
			UIUtilities.TypeIntoTextbox(window.textbox_FamilyMemberOQ, "1234560");

			CheckAddPatientButtonState(window, true);

			//	Remove one value at a time from the textboxes
			//	Client OQ		
			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, "");
			CheckAddPatientButtonState(window, false);

			UIUtilities.TypeIntoTextbox(window.textbox_ClientOQ, tempPatient.PatientOQ);

			//	First Name
			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, "");
			CheckAddPatientButtonState(window, false);

			UIUtilities.TypeIntoTextbox(window.textbox_FirstName, tempPatient.PatientFirstName);

			//	Last Name
			UIUtilities.TypeIntoTextbox(window.textbox_LastName, "");
			CheckAddPatientButtonState(window, false);

			UIUtilities.TypeIntoTextbox(window.textbox_LastName, tempPatient.PatientLastName);

			//	Relation
			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, "");
			CheckAddPatientButtonState(window, false);

			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, tempPatient.RelationToHead);

			//	Family OQ
			UIUtilities.TypeIntoTextbox(window.textbox_FamilyMemberOQ, "");
			CheckAddPatientButtonState(window, false);

			UIUtilities.TypeIntoTextbox(window.textbox_FamilyMemberOQ, "1234560");

			//	Clear Head of House, then Check the checkbox
			UIUtilities.TypeIntoTextbox(window.textbox_RelationToHead, "");

			UIUtilities.ClickOnItem(window.check_HeadOfHousehold);
			CheckAddPatientButtonState(window, true);

			//	Clear family relation, then Check the checkbox
			UIUtilities.TypeIntoTextbox(window.textbox_FamilyMemberOQ, "");

			UIUtilities.ClickOnItem(window.check_FirstHouseholdMember);
			CheckAddPatientButtonState(window, true);

			//	With the new household selected, test the textbox
			UIUtilities.TypeIntoTextbox(window.textbox_HouseholdPopulation, "");
			CheckAddPatientButtonState(window, false);

			UIUtilities.CloseWindow(window);
		}

	//	Used in the TestButtonDisable test
		private void CheckAddPatientButtonState(Window_Client window, bool isEnabled)
		{
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.button_AddUpdateClient.IsLoaded)));
			
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual(isEnabled, (bool)window.button_AddUpdateClient.IsEnabled);
			}));
		}

	/// <summary>
	/// Opens CreateNewPatient dialog, then returns the object
	/// </summary>
	/// <returns></returns>
		private Window_Client OpenCreateNewPatient()
		{
			Window_Client window = null;

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				window = new Window_Client(Definition.Admin, null);
				window.Show();
				window.Activate();
			}));

			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.IsLoaded)));

			return window;
		}

		private Test_Patient OpenTestPatient()
		{
			Test_Patient test = null;

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				test = new Test_Patient();
				test.Show();
			}));

			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => test.IsLoaded)));

			return test;
		}

		private void FindPatient(Test_Patient test, string patientOQ)
		{
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				test.Activate();
			}));

			UIUtilities.TypeIntoTextbox(test.text_PatientOQ, patientOQ);

			UIUtilities.ClickOnItem(test.button_GetPatient);
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => test.IsActive)));
		}

		private void DeletePatient(Test_Patient test, string patientOQ)
		{
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				test.Activate();
			}));

			UIUtilities.TypeIntoTextbox(test.text_PatientOQ, patientOQ);

			UIUtilities.ClickOnItem(test.button_DeletePatient);
		}

		private void DeleteHousehold(Test_Patient test, string patientOQ)
		{
			int householdID = 0;

			FindPatient(test, patientOQ);
			
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				householdID = int.Parse(test.text_HouseholdID.Text);
			}));

			DeleteHousehold(test, householdID);
		}

		private void DeleteHousehold(Test_Patient test, int HouseholdID)
		{
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				test.Activate();
			}));

			UIUtilities.TypeIntoTextbox(test.text_HouseholdID, HouseholdID.ToString());

			UIUtilities.ClickOnItem(test.button_DeleteHousehold);
		}

		private void AddPatient(Test_Patient test, Patient tempPatient, PatientHousehold household)
		{
			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				test.Activate();
			}));

			UIUtilities.TypeIntoTextbox(test.text_AgeGroup, tempPatient.PatientAgeGroup);
			
			UIUtilities.TypeIntoTextbox(test.text_Ethnicity, tempPatient.PatientEthnicity);
			UIUtilities.TypeIntoTextbox(test.text_FirstName, tempPatient.PatientFirstName);
			UIUtilities.TypeIntoTextbox(test.text_Gender, tempPatient.PatientGender);
			UIUtilities.TypeIntoTextbox(test.text_LastName, tempPatient.PatientLastName);
			UIUtilities.TypeIntoTextbox(test.text_PatientOQ, tempPatient.PatientOQ);
			UIUtilities.TypeIntoTextbox(test.text_RelationToHEad, tempPatient.RelationToHead);

			if (tempPatient.IsHead)
			{
				UIUtilities.ClickOnItem(test.check_IsHead);
			}
			
			if (tempPatient.HouseholdID == 0)
			{
				UIUtilities.ClickOnItem(test.check_NewHousehold);
				UIUtilities.TypeIntoTextbox(test.text_county, household.HouseholdCounty);
				UIUtilities.TypeIntoTextbox(test.text_Income, household.HouseholdIncomeBracket);
				UIUtilities.TypeIntoTextbox(test.text_HouseholdPop, household.HouseholdPopulation.ToString());
			}
			else
			{
				UIUtilities.TypeIntoTextbox(test.text_HouseholdID, tempPatient.HouseholdID.ToString());
			}

			UIUtilities.ClickOnItem(test.button_AddPatient);
		}
	}
}
