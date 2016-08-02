//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using FCS_DataTesting;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateDonor.xaml
    /// </summary>
    public partial class UpdateDonor : Window
    {
        public string DonorAddress1 { get; set; }
        public string DonorAddress2 { get; set; }
        public string DonorCity { get; set; }
        public string DonorState { get; set; }
        public string DonorType { get; set; }
        public string DonorZip { get; set; }
        public string OrganizationName { get; set; }
        private int DonorID { get; set; }
        
        public string StaffDBRole { get; set; }

        public UpdateDonor(DonorsDataGrid d , string StaffRole)
        {
            StaffDBRole = StaffRole;
            DonorAddress1 = d.DonorAddress1;
            DonorAddress2 = d.DonorAddress2;
            DonorCity = d.DonorCity;
            DonorState = d.DonorState;
            DonorType = d.DonorType;
            DonorZip = d.DonorZip;
            DonorID = d.DonorID;
            OrganizationName = d.OrganizationName;
            InitializeComponent();
			text_OrganizationName.Focus();
        }

        private void Update_Donor(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var donor = (from p in db.Donors
                           where p.DonorID == DonorID
                           select p).First();
            
            donor.DonorAddress1 = DonorAddress1;
            donor.DonorAddress2 = DonorAddress2;
            donor.DonorCity = DonorCity;
            donor.DonorState = DonorState;
            donor.DonorZip = DonorZip;
            donor.OrganizationName = OrganizationName;
            donor.DonorType = DonorType;
            int changes = db.SaveChanges();
            

            this.Close();
        }

        private void Delete_Donor(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Donor?" , "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                var donorContact = (from p in db.DonorContacts
                                    where p.DonorID == DonorID
                                    select p);
                foreach(Models.DonorContact contact in donorContact)
                {
                    db.DonorContacts.Remove(contact);
                }
                var donor = (from p in db.Donors
                               where p.DonorID == DonorID
                               select p).First();

                db.Donors.Remove(donor);
                db.SaveChanges();
                
                this.Close();
            }
        }
        private void Refresh_ContactsGrid(object sender, RoutedEventArgs e)
        {
            sender = ContactsGrid;
            Load_Contacts_Grid(sender, e);
        }
        private void Load_Contacts_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var join1 = from c in db.DonorContacts
                        where c.DonorID == DonorID
                        select new DonorContactGrid
                        {
                            ContactFirstName = c.ContactFirstName,
                            ContactLastName = c.ContactLastName,
                            ContactPhone = c.ContactPhone,
                            ContactEmail = c.ContactEmail,
                            DonorID = c.DonorID,
                            ContactID = c.ContactID
                        };
            var grid = sender as DataGrid;
            if (grid == null)
            {
                ContactsGrid.ItemsSource = join1.ToList();
            }
            else
            {
                ContactsGrid.ItemsSource = join1.ToList();
            }
        }

        private void Add_New_Contact(object sender, RoutedEventArgs e)
        {
            AddNewContact anc = new AddNewContact(DonorID);
            anc.ShowDialog();
            
            Refresh_ContactsGrid(sender, e);
        }

        private void Edit_Contact(object sender, MouseButtonEventArgs e)
        {
			try
			{
				DataGrid dg = sender as DataGrid;
				DonorContactGrid p = (DonorContactGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateContact up = new UpdateContact(p);
				up.DeleteCon.IsEnabled = false;
				up.ShowDialog();
			}
			catch (Exception error)
			{
			}       
			Refresh_ContactsGrid(sender, e);
          }
        

        private void AddNewDonation(object sender, RoutedEventArgs e)
        {
            CreateMoneyDonation cmd = new CreateMoneyDonation(DonorID, false, -1);
            cmd.ShowDialog();

            Refresh_DonationsGrid(sender, e);
        }

        private void EditDonation(object sender, MouseButtonEventArgs e)
        {

                try {
                    DataGrid dg = sender as DataGrid;
                    DonationsGrid p = (DonationsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                    UpdateDonation up = new UpdateDonation(p);
                    up.DeleteDon.IsEnabled = false;
                    up.DonationDate.SelectedDate = p.DonationDate;
                    up.ShowDialog();

                    Refresh_DonationsGrid(sender, e);
            }
                catch
                {

                }
        }

        private void Refresh_DonationsGrid(object sender, RoutedEventArgs e)
        {
            sender = DonorGrid;
            Donations_Grid(sender, e);
        }

        private void Donations_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();

            var join1 = from d in db.Donations
                        where d.DonorID == DonorID                                                
                        select new DonationsGrid
                        {
                            DonationAmount = d.DonationAmount,
                            DonationAmountRemaining = d.DonationAmountRemaining,
                            DonationDate = d.DonationDate,
                            DonorID = d.DonorID,
                            DonationID = d.DonationID
                        };

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
