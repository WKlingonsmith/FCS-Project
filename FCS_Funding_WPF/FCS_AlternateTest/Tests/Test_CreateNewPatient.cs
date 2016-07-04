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
		public void TestAddingPatient()
		{
			//	Set the testing data
			Patient tempPatient = new Patient();
			tempPatient.PatientOQ = "1234567890";
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
			UIUtilities.ClickOnItem(window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			KeyboardUtilities.SendKeys(tempPatient.PatientOQ);

			UIUtilities.ClickOnTextbox(window.textbox_FirstName);
			KeyboardUtilities.SendKeys(tempPatient.PatientFirstName);

			UIUtilities.ClickOnTextbox(window.textbox_LastName);
			KeyboardUtilities.SendKeys(tempPatient.PatientLastName);

			UIUtilities.ClickOnTextbox(window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys(tempPatient.RelationToHead);

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

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				tpat.Close();
			}));
		}

	/// <summary>
	/// Tests that the number-only textboxes can only hold numbers
	/// </summary>
		[TestMethod]
		public void TestNumberOnlyInput()
		{
			Window_Client window = OpenCreateNewPatient();

			//	Client OQ Textbo
			UIUtilities.ClickOnTextbox(window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			UIUtilities.ClickOnTextbox(window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("123456789abcdefghijklmnopqrstuvwxyz");

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_ClientOQ.Text);
			}));

			//	Family OQ Textbox
			UIUtilities.ClickOnTextbox(window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("123456789abcdefghijklmnopqrstuvwxyz");

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_FamilyMemberOQ.Text);
			}));

			//	Household Pop
			UIUtilities.ClickOnItem(window.check_FirstHouseholdMember);
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.IsLoaded)));

			UIUtilities.ClickOnTextbox(window.textbox_HouseholdPopulation);
			KeyboardUtilities.SendKeys("123456789abcdefghijklmnopqrstuvwxyz");
		}

		[TestMethod]
		public void TestButtonDisable()
		{
			Window_Client window = OpenCreateNewPatient();

			//	Check that the button is initially disabled
			CheckAddPatientButtonState(window, false);

			//	Type into all other entry points, so the button is enabled
			UIUtilities.ClickOnTextbox(window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("1234560");

			UIUtilities.ClickOnTextbox(window.textbox_FirstName);
			KeyboardUtilities.SendKeys("Test");

			UIUtilities.ClickOnTextbox(window.textbox_LastName);
			KeyboardUtilities.SendKeys("McGee");

			UIUtilities.ClickOnTextbox(window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("Related");

			UIUtilities.ClickOnTextbox(window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("1234550");

			CheckAddPatientButtonState(window, true);

			//	Remove one value at a time from the textboxes
			//	Client OQ		
			UIUtilities.ClickOnTextbox(window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			UIUtilities.ClickOnTextbox(window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("1234560");

			//	First Name
			UIUtilities.ClickOnTextbox(window.textbox_FirstName);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			UIUtilities.ClickOnTextbox(window.textbox_FirstName);
			KeyboardUtilities.SendKeys("Test");

			//	Last Name			
			UIUtilities.ClickOnTextbox(window.textbox_LastName);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			UIUtilities.ClickOnTextbox(window.textbox_LastName);
			KeyboardUtilities.SendKeys("McGee");

			//	Relation
			UIUtilities.ClickOnTextbox(window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			UIUtilities.ClickOnTextbox(window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("Related");

			//	Family OQ
			UIUtilities.ClickOnTextbox(window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			UIUtilities.ClickOnTextbox(window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("1234550");

			//	Clear Head of House, then Check the checkbox
			UIUtilities.ClickOnTextbox(window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();

			UIUtilities.ClickOnItem(window.check_HeadOfHousehold);
			CheckAddPatientButtonState(window, true);

			//	Clear family relation, then Check the checkbox
			UIUtilities.ClickOnTextbox(window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();

			UIUtilities.ClickOnItem(window.check_FirstHouseholdMember);
			CheckAddPatientButtonState(window, true);

			//	With the new household selected, test the textbox
			UIUtilities.ClickOnTextbox(window.textbox_HouseholdPopulation);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);
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
	}
}
