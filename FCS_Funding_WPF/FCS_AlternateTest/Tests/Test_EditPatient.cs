using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCS_Funding.Models;
using FCS_Funding.Views.TestWindows;
using UnitTestUtilities;
using FCS_Funding;
using FCS_Funding.Views.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FCS_AlternateTest.Tests
{
	using Definition;
	using System.Windows;
	class Test_EditPatient
	{
		[TestMethod]
		public void EditPatient()
		{
			//todo
		}

		[TestMethod]
		public void EditPatientWrongFamilyOQ()
		{
			//todo
		}

		[TestMethod]
		public void EditPatientCreateNewHousehold()
		{
			//todo
		}

		[TestMethod]
		public void EditPatientChangeHousehold()
		{
			//todo
		}

		[TestMethod]
		public void EditPatientUpdateHousehold()
		{
			//todo
		}

		[TestMethod]
		public void DeletePatient()
		{
			//todo
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
