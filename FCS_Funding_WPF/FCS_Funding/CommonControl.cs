using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FCS_Funding
{
	class CommonControl
	{
		public static bool IsTextNumeric(string strTextToCheck)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9.]+");
			return !regex.IsMatch(strTextToCheck);
		}

		public static bool IsTextNumericNoPeriod(string strTextToCheck)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
			return !regex.IsMatch(strTextToCheck);
		}

		/// <summary>
		/// This should be calle din an event to ensure that only numbers and periods are included
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void NumberOnlyEventCheck(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !CommonControl.IsTextNumeric(e.Text);
		}

		/// <summary>
		/// This should be called in an event to just ensure that only numbers are included
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void NumberOnlyEventCheckNoPeriod(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !CommonControl.IsTextNumericNoPeriod(e.Text);
		}

		public static void IntepretEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			var original = e.OriginalSource as FrameworkElement;

			if (e.Key == Key.Enter)
			{
				e.Handled = true;

				original.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
		}

	}
}
