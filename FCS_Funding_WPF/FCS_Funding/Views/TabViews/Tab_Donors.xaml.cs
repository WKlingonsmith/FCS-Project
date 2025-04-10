﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FCS_DataTesting;
using FCS_Funding.Models;

namespace FCS_Funding.Views.TabViews
{
	using Definition;

	/// <summary>
	/// Interaction logic for Tab_Donor.xaml
	/// </summary>
	public partial class Tab_Donors : UserControl
	{
		public string StaffRole { get; set; }

		public Tab_Donors()
		{
			InitializeComponent();
		}


		private void EditDonor(object sender, MouseButtonEventArgs e)
		{
			try
			{
				DataGrid dg = sender as DataGrid;
				DonorsDataGrid p = (DonorsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				var db = new FCS_DBModel();

				if (p.DonorType == "Individual")
				{
					//Open in individual view
					Models.DonorContact query = (from doncontacts in db.DonorContacts
													where doncontacts.DonorID == p.DonorID
													select doncontacts).First();
					UpdateIndividualDonor id = new UpdateIndividualDonor(p, query, StaffRole);
					id.dType.SelectedIndex = 1;
					id.oName.IsEnabled = false;
					id.ShowDialog();
				}
				else if (p.DonorType == "Anonymous")
				{
					Models.DonorContact query = (from doncontacts in db.DonorContacts
													where doncontacts.DonorID == p.DonorID
													select doncontacts).First();
					UpdateIndividualDonor id = new UpdateIndividualDonor(p, query, StaffRole);

					id.ShowDialog();
					id.UpdateIndDonor.IsEnabled = false;
					id.dType.SelectedIndex = 2;
					id.fName.IsEnabled = false;
					id.lName.IsEnabled = false;
					id.oName.IsEnabled = false;
					id.donA1.IsEnabled = false;
					id.donA2.IsEnabled = false;
					id.cPhone.IsEnabled = false;
					id.dCity.IsEnabled = false;
					id.cPhone.IsEnabled = false;
					id.dState.IsEnabled = false;
					id.dZip.IsEnabled = false;
					id.cEmail.IsEnabled = false;

				}
				else
				{
					UpdateDonor up = new UpdateDonor(p, StaffRole);

					up.ShowDialog();
				}
			}
			catch
			{
			}

			//	Refresh the grid after editing
			Refresh_DonorGrid(sender, e);
		}


		private void Open_CreateNewDonor(object sender, RoutedEventArgs e)
		{
			CreateNewDonor ch = new CreateNewDonor();
			ch.ShowDialog();

			//	Refresh the grid
			Refresh_DonorGrid(sender, e);
		}

		private void Open_AddPurpose(object sender, RoutedEventArgs e)
		{
			AddPurpose ap = new AddPurpose();
			ap.ShowDialog();
		}

		private void EditDonation(object sender, MouseButtonEventArgs e)
		{
			try
			{
				DataGrid dg = sender as DataGrid;

				DonationsGrid p = (DonationsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateDonation up = new UpdateDonation(p);
				
				up.DonationDate.SelectedDate = p.DonationDate;
				up.ShowDialog();
			}
			catch
			{
			}
			Refresh_DonationGrid(sender, e);
		}
		private void Refresh_DonorGrid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = (from d in db.Donors
						 join dc in db.DonorContacts on d.DonorID equals dc.DonorID
						 where d.DonorType == "Anonymous" || d.DonorType == "Individual"
						 select new DonorsDataGrid
						 {
							 DonorID = d.DonorID,
							 DonorFirstName = dc.ContactFirstName,
							 DonorLastName = dc.ContactLastName,
							 DonorAddress1 = d.DonorAddress1,
							 OrganizationName = "",
							 DonorAddress2 = d.DonorAddress2,
							 DonorCity = d.DonorCity,
							 DonorState = d.DonorState,
							 DonorType = d.DonorType,
							 DonorZip = d.DonorZip
						 }).Union(
						from d in db.Donors
						where d.DonorType == "Organization" || d.DonorType == "Government" || d.DonorType == "Insurance"
						select new DonorsDataGrid
						{
							DonorID = d.DonorID,
							DonorFirstName = "",
							DonorLastName = "",
							DonorAddress1 = d.DonorAddress1,
							OrganizationName = d.OrganizationName,
							DonorAddress2 = d.DonorAddress2,
							DonorCity = d.DonorCity,
							DonorState = d.DonorState,
							DonorType = d.DonorType,
							DonorZip = d.DonorZip
						});

			Donor_DataGrid.ItemsSource = join1.ToList();
		}

		private void Refresh_DonationGrid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join2 = (from d in db.Donations
						 join dn in db.Donors
						 on d.DonorID equals dn.DonorID
						 where (dn.DonorType == "Organization" || dn.DonorType == "Government" || dn.DonorType == "Insurance")
						 select new DonationsGrid
						 {
							 DonationAmount = d.DonationAmount,
							 DonationAmountRemaining = d.DonationAmountRemaining,
							 DonationDate = d.DonationDate,
							 DonorID = d.DonorID,
							 DonationID = d.DonationID,
							 DonorFirstName = "",
							 DonorLastName = "",
							 OrganizationName = dn.OrganizationName,

						 }).Union(from d in db.Donations
								  join dn in db.Donors on d.DonorID equals dn.DonorID
								  join c in db.DonorContacts on dn.DonorID equals c.DonorID
								  where (dn.DonorType == "Anonymous" || dn.DonorType == "Individual")
								  && d.GrantProposalID == null
								  select new DonationsGrid
								  {
									  DonationAmount = d.DonationAmount,
									  DonationAmountRemaining = d.DonationAmountRemaining,
									  DonationDate = d.DonationDate,
									  DonorID = d.DonorID,
									  DonationID = d.DonationID,
									  DonorFirstName = c.ContactFirstName,
									  DonorLastName = c.ContactLastName,
									  OrganizationName = "",
								  });
								  
			Donation_DataGrid.ItemsSource = join2.ToList();
		}
	}
}
