using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UnitTestUtilities
{
	public static class UIUtilities
	{
		public static void ClickOnItemNoWait(UIElement element)
		{
			Point middle = new Point();
			ThreadUtilities.RunOnUIThread(new Action(() => middle = GeneralUtilities.GetMiddleInScreenCoordinates(element)));
			MouseUtilities.LeftClickScreen((int)middle.X, (int)middle.Y);
		}

		/// <summary>
		/// Clicks any given item. If you are clicking on a textbox, you should use the ClickOnTextbox method.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="window_checkbox"></param>
		public static void ClickOnItem(UIElement element)
		{
			ClickOnItemNoWait(element);
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => element.IsFocused)));
		}

		/// <summary>
		/// Clicks in a Textbox so you have focus, then use SendKeys to type into the textbox
		/// </summary>
		/// <param name="window"></param>
		/// <param name="window_textbox"></param>
		public static void ClickOnTextbox(TextBox window_textbox)
		{
			ClickOnItem(window_textbox);
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window_textbox.IsKeyboardFocusWithin)));
		}

		public static void TypeIntoTextbox(TextBox window_textbox, string valueToEnter)
		{
			UIUtilities.ClickOnItem(window_textbox);
			KeyboardUtilities.SendKeys("^a");
			KeyboardUtilities.SendBackspace();
			KeyboardUtilities.SendKeys(valueToEnter);
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window_textbox.Text == valueToEnter)));
		}

		/// <summary>
		/// Selects an item from a combobox
		/// </summary>
		/// <param name="window_combobox"></param>
		/// <param name="item"></param>
		public static void SelectComboboxItem(ComboBox window_combobox, string item)
		{
			ThreadUtilities.RunOnUIThread(new Action(() => window_combobox.Text = item));
			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window_combobox.IsLoaded)));
		}
	}
}
