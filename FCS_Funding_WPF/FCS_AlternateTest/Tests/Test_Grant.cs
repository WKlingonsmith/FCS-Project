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
using FCS_Funding.Views;
using System.Windows;

namespace FCS_AlternateTest.Tests
{
	class Test_Grant
	{
		[TestMethod]
		public void AddNewGrantProposal()
		{

		}

		[TestMethod]
		public void EditGrantProposalStatusToNotAccepted()
		{

		}

		[TestMethod]
		public void EditGrantProposalStatusToAccepted()
		{

		}

		[TestMethod]
		public void UpdateApprovedGrant()
		{

		}
		

		[TestMethod]
		public void DeleteGrantProposal()
		{

		}

		private CreateGrantProposal OpenNewGrantProposal()
		{
			CreateGrantProposal window = null;

			ThreadUtilities.RunOnUIThread(new Action(() =>
			{
				window = new CreateGrantProposal();
				window.Show();
				window.Activate();
			}));

			GeneralUtilities.WaitUntil(() => (bool)Application.Current.Dispatcher.Invoke(new Func<bool>(() => window.IsLoaded)));

			return window;
		}
	}
}
