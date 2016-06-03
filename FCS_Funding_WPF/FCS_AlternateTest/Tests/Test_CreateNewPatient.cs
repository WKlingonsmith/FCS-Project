using System;
using FCS_Funding;
using FCS_Funding.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnitTestUtilities;
using System.Windows.Controls;

namespace FCS_AlternateTest
{
	[TestClass]
	public class UnitTest1
	{
	/// <summary>
	/// Tests that the number-only textboxes can only hold numbers
	/// </summary>
		[TestMethod]
		public void TestNumberOnlyInput()
		{
			CreateNewPatient window = OpenCreateNewPatient();

		//	Client OQ Textbo
			ClickTextBox(window, window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("123456789abcdefghijklmnopqrstuvwxyz");

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_ClientOQ.Text);
			}));

			//	Family OQ Textbox
			ClickTextBox(window, window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("123456789abcdefghijklmnopqrstuvwxyz");

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual("123456789", window.textbox_FamilyMemberOQ.Text);
			}));

			//	Household Pop
			ClickCheckbox(window, window.check_FirstHouseholdMember);

			ClickTextBox(window, window.textbox_HouseholdPopulation);
			KeyboardUtilities.SendKeys("123456789abcdefghijklmnopqrstuvwxyz");
		}

		[TestMethod]
		public void TestButtonDisable()
		{
			CreateNewPatient window = OpenCreateNewPatient();

			//	Check that the button is initially disabled
			CheckAddPatientButtonState(window, false);

			//	Type into all other entry points, so the button is enabled
			ClickTextBox(window, window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("1234560");

			ClickTextBox(window, window.textbox_FirstName);
			KeyboardUtilities.SendKeys("Test");

			ClickTextBox(window, window.textbox_LastName);
			KeyboardUtilities.SendKeys("McGee");

			ClickTextBox(window, window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("Related");

			ClickTextBox(window, window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("1234550");

			CheckAddPatientButtonState(window, true);

		//	Remove one value at a time from the textboxes
			//	Client OQ		
			ClickTextBox(window, window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			ClickTextBox(window, window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("1234560");

			//	First Name
			ClickTextBox(window, window.textbox_FirstName);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			ClickTextBox(window, window.textbox_FirstName);
			KeyboardUtilities.SendKeys("Test");

			//	Last Name			
			ClickTextBox(window, window.textbox_LastName);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			ClickTextBox(window, window.textbox_LastName);
			KeyboardUtilities.SendKeys("McGee");

			//	Relation
			ClickTextBox(window, window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			ClickTextBox(window, window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("Related");

			//	Family OQ
			ClickTextBox(window, window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);

			ClickTextBox(window, window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("1234550");

			//	Clear Head of House, then Check the checkbox
			ClickTextBox(window, window.textbox_RelationToHead);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();

			ClickCheckbox(window, window.check_HeadOfHousehold);
			CheckAddPatientButtonState(window, true);

			//	Clear family relation, then Check the checkbox
			ClickTextBox(window, window.textbox_FamilyMemberOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();

			ClickCheckbox(window, window.check_FirstHouseholdMember);
			CheckAddPatientButtonState(window, true);

			//	With the new household selected, test the textbox
			ClickTextBox(window, window.textbox_HouseholdPopulation);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			CheckAddPatientButtonState(window, false);
		}

	//	Used in the TestButtonDisable test
		private void CheckAddPatientButtonState(CreateNewPatient window, bool isEnabled)
		{
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.button_AddClient.IsLoaded)));

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual(isEnabled, (bool)window.button_AddClient.IsEnabled);
			}));
		}

	/// <summary>
	/// Clicks in a Textbox so you have focus, then use SendKeys to type into the textbox
	/// </summary>
	/// <param name="window"></param>
	/// <param name="window_textbox"></param>
		private void ClickTextBox(CreateNewPatient window, TextBox window_textbox)
		{
			Point middle = new Point();
			ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(window_textbox)));
			MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window_textbox.IsKeyboardFocusWithin)));
		}

	/// <summary>
	/// Clicks on a checkbox
	/// </summary>
	/// <param name="window"></param>
	/// <param name="window_checkbox"></param>
		private void ClickCheckbox(CreateNewPatient window, CheckBox window_checkbox)
		{
			Point middle = new Point();
			ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(window_checkbox)));
			MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
		}

	/// <summary>
	/// Opens CreateNewPatient dialog, then returns the object
	/// </summary>
	/// <returns></returns>
		private CreateNewPatient OpenCreateNewPatient()
		{
			CreateNewPatient window = null;

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				window = new CreateNewPatient();
				window.Show();
			}));

			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.IsLoaded)));

			return window;
		}
	}
}
