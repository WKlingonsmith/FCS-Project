using System;
using FCS_Funding;
using FCS_Funding.Views.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnitTestUtilities;
using System.Windows.Controls;

namespace FCS_AlternateTest
{
	using Definition;

	[TestClass]
	public class UnitTest1
	{
	/// <summary>
	/// Tests that the number-only textboxes can only hold numbers
	/// </summary>
		[TestMethod]
		public void TestNumberOnlyInput()
		{
			Window_Client window = OpenCreateNewPatient();

			//	Client OQ Textbo
			ClickTextBox(window, window.textbox_ClientOQ);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
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
			Window_Client window = OpenCreateNewPatient();

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
		private void CheckAddPatientButtonState(Window_Client window, bool isEnabled)
		{
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.button_AddUpdateClient.IsLoaded)));

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				Assert.AreEqual(isEnabled, (bool)window.button_AddUpdateClient.IsEnabled);
			}));
		}

	/// <summary>
	/// Clicks in a Textbox so you have focus, then use SendKeys to type into the textbox
	/// </summary>
	/// <param name="window"></param>
	/// <param name="window_textbox"></param>
		private void ClickTextBox(Window_Client window, TextBox window_textbox)
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
		private void ClickCheckbox(Window_Client window, CheckBox window_checkbox)
		{
			Point middle = new Point();
			ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(window_checkbox)));
			MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
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
			}));

			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.IsLoaded)));

			return window;
		}
	}
}
