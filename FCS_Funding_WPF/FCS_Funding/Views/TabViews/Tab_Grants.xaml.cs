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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FCS_DataTesting;
using FCS_Funding.Models;

namespace FCS_Funding.Views.TabViews
{
	using Definition;
	using System.Collections.ObjectModel;
	/// <summary>
	/// Interaction logic for Tab_Grants.xaml
	/// </summary>
	public partial class Tab_Grants : UserControl
	{
		public string StaffRole { get; set; }

		public Tab_Grants()
		{
			InitializeComponent();

			//	Check for permissions
			StaffRole = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().StaffDBRole;

			if (StaffRole == Definition.Basic || StaffRole == Definition.User)
			{
				CreateGrantProp.IsEnabled = false;
			}
			else if (StaffRole == Definition.Admin)
			{
				CreateGrantProp.IsEnabled = true;
			}
		}

		private void EditGrant(object sender, MouseButtonEventArgs e)
		{
			var db = new FCS_DBModel();

			if (StaffRole != Definition.Basic)
			{
				DataGrid dg = sender as DataGrid;

				GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateGrant up = new UpdateGrant(p);
				if (StaffRole != Definition.Admin)
				{
					up.DeleteGran.IsEnabled = false;
				}
				//Grant prop ID & donation ID with expense
				//p.DonationID
				var expenseTotal = (from ex in db.Expenses
									where ex.DonationID == p.DonationID
									select ex).Count();
				if (expenseTotal > 0) { up.DonAmount.IsEnabled = false; up.AmountRem.IsEnabled = false; }
				up.DonationDate.SelectedDate = p.DonationDate;
				up.DonationExpiration.SelectedDate = p.ExpirationDate;
				up.Topmost = true;
				up.ShowDialog();
			}
		}

		private void Grants_Grid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = from d in db.Donations
						join dr in db.Donors on d.DonorID equals dr.DonorID
						join gp in db.GrantProposals on dr.DonorID equals gp.DonorID
						where gp.GrantStatus == "Accepted" && d.GrantProposalID == gp.GrantProposalID
						select new GrantsDataGrid
						{
							GrantName = gp.GrantName,
							DonationAmount = d.DonationAmount,
							DonationAmountRemaining = d.DonationAmountRemaining,
							DonationDate = d.DonationDate,
							ExpirationDate = d.DonationExpirationDate,
							DonationID = d.DonationID,
							DonorID = dr.DonorID,
							GrantProposalID = gp.GrantProposalID
						};

			GrantsDataGrid g1 = new GrantsDataGrid("Cross Charitable Foundation", 1024.25M, DateTime.Now, DateTime.Now, "Charitable Minds", "We wanted to donate", 1024.25M);
			var Grants = new ObservableCollection<GrantsDataGrid>();
			Grants.Add(g1);

			// ... Assign ItemsSource of DataGrid.
			var grid = sender as DataGrid;
			grid.ItemsSource = join1.ToList();
		}

		private void Open_CreateGrantProposal(object sender, RoutedEventArgs e)
		{
			CreateGrantProposal cgp = new CreateGrantProposal();
			cgp.ShowDialog();
		}

		private void Open_ViewGrantProposals(object sender, RoutedEventArgs e)
		{
			ViewGrantProposals vgp = new ViewGrantProposals(StaffRole);
			vgp.ShowDialog();
		}

		private void Refresh_Grants(object sender, RoutedEventArgs e)
		{
			sender = Grant_DataGrid;
			Grants_Grid(sender, e);
		}

	}
}
