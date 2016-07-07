using System;
using FCS_Funding;
using FCS_Funding.Views.Windows;
using FCS_Funding.Views.TabViews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnitTestUtilities;

namespace FCS_AlternateTest
{
    using Definition;
    using System.Windows.Controls;
    using System.Windows.Input;

    [TestClass]
    public class Test_CreateNewPatient
    {
        //[TestMethod]
        //public void check_if_add_button_is_disabled()
        //{
        //    Window_Client window = OpenCreateNewPatient();
        //    CheckAddPatientButtonState(window, false);


        //}
        [TestMethod]
        public void Add_New_Patient_as_Head_Of_House()
        {
            Window_Client window = OpenCreateNewPatient();
            

            ClickTextBox(window, window.textbox_ClientOQ);
            KeyboardUtilities.SendKeys("^a");
            KeyboardUtilities.SendBackspace();
            KeyboardUtilities.SendKeys("9");

            ClickTextBox(window, window.textbox_FirstName);
            KeyboardUtilities.SendKeys("First_name");

            ClickTextBox(window, window.textbox_LastName);
            KeyboardUtilities.SendKeys("Last_name");

            ClickCheckbox(window, window.check_HeadOfHousehold);

            ClickCheckbox(window, window.check_FirstHouseholdMember);

            CheckAddPatientButtonState(window, true);

            ClickButton(window, window.button_AddUpdateClient);

           
        }

        //[TestMethod]
        //public void Add_New_Patient_not_head_of_House()
        //{
        //    Window_Client window = OpenCreateNewPatient();

            
        //}

        private void CheckAddPatientButtonState(Window_Client window, bool isEnabled)
        {
            GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.button_AddUpdateClient.IsLoaded)));

            ThreadUtilities.RunOnUIThread(new Action(() =>
            {
                Assert.AreEqual(isEnabled, (bool)window.button_AddUpdateClient.IsEnabled);
            }));
        }

        private void ClickButton(Window_Client window, Button window_Button)
        {
            Point middle = new Point();
            ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(window_Button)));
            MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
            
        }

        private void ClickTextBox(Window_Client window, TextBox window_textbox)
        {
            Point middle = new Point();
            ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(window_textbox)));
            MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
            GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window_textbox.IsKeyboardFocusWithin)));
        }

        private void ClickCheckbox(Window_Client window, CheckBox window_checkbox)
        {
            Point middle = new Point();
            ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(window_checkbox)));
            MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
        }

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
        private Tab_Clients OpenTabClients()
        {
            Tab_Clients window = null;

            //ThreadUtilities.RunOnUIThread(new Action(() =>
            //{
            //    window = new Tab_Clients();
            //    //window.Show();
            //}));

            //GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.IsLoaded)));

            return window;
        }


    }
}

