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

		public Tab_Grants()
		{
			InitializeComponent();
		}

		private void EditGrant(object sender, MouseButtonEventArgs e)
		{
			var db = new FCS_DBModel();
			
			try
			{
				DataGrid dg = sender as DataGrid;

				GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateGrant up = new UpdateGrant(p);
				//Grant prop ID & donation ID with expense
				//p.DonationID
				var expenseTotal = (from ex in db.Expenses
									where ex.DonationID == p.DonationID
									select ex).Count();
				if (expenseTotal > 0) { up.DonAmount.IsEnabled = false; up.AmountRem.IsEnabled = false; }
				up.DonationDate.SelectedDate = p.DonationDate;
				up.DonationExpiration.SelectedDate = p.ExpirationDate;
				up.ShowDialog();
			}
			catch (Exception error)
			{
			}

			Refresh_GrantGrid(sender, e);
		}

		private void Open_CreateGrantProposal(object sender, RoutedEventArgs e)
		{
			CreateGrantProposal cgp = new CreateGrantProposal();
			cgp.ShowDialog();

			Refresh_GrantGrid(sender, e);
		}

		private void Open_ViewGrantProposals(object sender, RoutedEventArgs e)
		{
			ViewGrantProposals vgp = new ViewGrantProposals(null);
			vgp.ShowDialog();

			Refresh_GrantGrid(sender, e);
		}

		private void Refresh_GrantGrid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = from d in db.Donations
						join dr in db.Donors on d.DonorID equals dr.DonorID
						join gp in db.GrantProposals on dr.DonorID equals gp.DonorID
                        join dp in db.DonationPurposes on d.DonationID equals dp.DonationID
                        join p in db.Purposes on dp.PurposeID equals p.PurposeID
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
							GrantProposalID = gp.GrantProposalID,
                            PurposeName = p.PurposeName,
                            PurposeDescription = p.PurposeDescription
                        };

			// ... Assign ItemsSource of DataGrid.
			Grant_DataGrid.ItemsSource = join1.ToList();
		}
	}
}
