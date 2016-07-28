
//using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using FCS_DataTesting;
using System;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateIndividualDonor.xaml
    /// </summary>
    public partial class UpdateIndividualDonor : Window
    {
        public string DonorFirstName    { get; set; }
        public string DonorLastName     { get; set; }
        public string OrganizationName  { get; set; }
        public string DonorAddress1     { get; set; }
        public string DonorAddress2     { get; set; }
        public string ContactPhone      { get; set; }
        public string ContactEmail      { get; set; }
        public string DonorState        { get; set; }
        public string DonorCity         { get; set; }
        public string DonorZip          { get; set; }
        public int    DonorID           { get; set; }
        public int ContactID { get; set; }

        public string StaffDBRole { get; set; }

        public UpdateIndividualDonor(DonorsDataGrid p, Models.DonorContact d, string staffDBRole)
        {
            DonorFirstName = p.DonorFirstName;
            DonorLastName = p.DonorLastName;
            OrganizationName = p.OrganizationName;
            DonorAddress1 = p.DonorAddress1;
            DonorAddress2 = p.DonorAddress2;
            ContactPhone = d.ContactPhone;
            ContactEmail = d.ContactEmail;
            DonorState = p.DonorState;
            DonorCity = p.DonorCity;
            DonorZip = p.DonorZip;
            DonorID = p.DonorID;
            ContactID = d.ContactID;
            StaffDBRole = staffDBRole;

            InitializeComponent();
			fName.Focus();
        }

        private void UpdateDonor(object sender, RoutedEventArgs e)
        {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
                var donor = (from d in db.Donors
                             where d.DonorID == DonorID
                             select d).First();
                var contact = (from c in db.DonorContacts
                               where c.ContactID == ContactID
                               select c).First();

                contact.ContactFirstName = DonorFirstName;
                contact.ContactLastName = DonorLastName;
                donor.OrganizationName = OrganizationName;
                donor.DonorAddress1 = DonorAddress1;
                donor.DonorAddress2 = DonorAddress2;
                contact.ContactPhone = ContactPhone;
                contact.ContactEmail = ContactEmail;
                donor.DonorState = DonorState;
                donor.DonorCity = DonorCity;
                donor.DonorZip = DonorZip;
                db.SaveChanges();
                MessageBox.Show("Donor updated successfully");
                this.Close();

        }

        private void DeleteDonor(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Donor?" , "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
                var donor = (from d in db.Donors
                             where d.DonorID == DonorID
                             select d).First();
                var contact = (from c in db.DonorContacts
                               where c.ContactID == ContactID
                               select c).First();
                db.DonorContacts.Remove(contact);
                db.Donors.Remove(donor);
                db.SaveChanges();
                
                this.Close();
            }
        }

        private void AddNewDonation(object sender, RoutedEventArgs e)
        {
            CreateMoneyDonation cmd = new CreateMoneyDonation(DonorID, false, -1);
            cmd.ShowDialog();
            
            Refresh_DonationGrid(sender, e);
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
                   
                    Refresh_DonationGrid(sender, e);
                }
                catch (Exception error)
                {
                    MessageBox.Show("could not edit");
                }

        }

            
        private void Refresh_DonationGrid(object sender, RoutedEventArgs e)
        {
            sender = grid_Donations;
            Donations_Grid(sender, e);
        }
        private void Donations_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();

            var join1 = (from d in db.Donations
                         where d.DonorID == DonorID
                         select new DonationsGrid
                         {
                             DonationAmount = d.DonationAmount,
                             DonationAmountRemaining = d.DonationAmountRemaining,
                             DonationDate = d.DonationDate,
                             PurposeName = (from p in db.Purposes
                                            join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                                            where dp.DonationID == d.DonationID
                                            select p.PurposeName).FirstOrDefault(),
                             PurposeDescription = (from p in db.Purposes
                                                   join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                                                   where dp.DonationID == d.DonationID
                                                   select p.PurposeDescription).FirstOrDefault(),
                             DonorID = d.DonorID,
                             DonationPurposeID = (from p in db.Purposes
                                                  join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                                                  where dp.DonationID == d.DonationID
                                                  select dp.DonationPurposeID).FirstOrDefault(),
                             PurposeID = (from p in db.Purposes
                                          join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                                          where dp.DonationID == d.DonationID
                                          select dp.PurposeID).FirstOrDefault(),
                             DonationID = d.DonationID
                         }).ToList();

            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
