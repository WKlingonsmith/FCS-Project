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
using System.Windows.Shapes;
using FCS_DataTesting;

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
        }

        private void Update_Donor(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
            var donor = (from p in db.Donors
                           where p.DonorID == DonorID
                           select p).First();
            //donor.DonorFirstName = DonorFirstName;
            //donor.DonorLastName = DonorLastName;
            donor.DonorAddress1 = DonorAddress1;
            donor.DonorAddress2 = DonorAddress2;
            donor.DonorCity = DonorCity;
            donor.DonorState = DonorState;
            donor.DonorZip = DonorZip;
            donor.OrganizationName = OrganizationName;
            donor.DonorType = DonorType;
            int changes = db.SaveChanges();
            MessageBox.Show("Updated these changes successfully.");
            this.Close();
        }

        private void Delete_Donor(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation", "Are you sure that you want to delete this Patient?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
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
                MessageBox.Show("You successfully deleted this Patient.");
                this.Close();
            }
        }

        private void Load_Contacts_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
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
            anc.Show();
            anc.Topmost = true;
        }

        private void Edit_Contact(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 2)
            {
                DataGrid dg = sender as DataGrid;
                DonorContactGrid p = (DonorContactGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateContact up = new UpdateContact(p);
                if (StaffDBRole != "Admin")
                {
                    up.DeleteCon.IsEnabled = false;
                }
                up.Show();
                this.Topmost = false;
                up.Topmost = true;
            }
        }

        private void AddNewDonation(object sender, RoutedEventArgs e)
        {
            CreateMoneyDonation cmd = new CreateMoneyDonation(DonorID);
            cmd.Show();
            cmd.Topmost = true;
        }

        private void EditDonation(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count <= 3)
            {
                DataGrid dg = sender as DataGrid;
                DonationsGrid p = (DonationsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateDonation up = new UpdateDonation(p);
                if (StaffDBRole != "Admin")
                {
                    up.DeleteDon.IsEnabled = false;
                }
                up.DonationDate.SelectedDate = p.DonationDate;
                up.Show();
                this.Topmost = false;
                up.Topmost = true;
            }
        }

        private void Donations_Grid(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
            var join1 = from p in db.Purposes
                        join dp in db.DonationPurposes on p.PurposeID equals dp.PurposeID
                        join d in db.Donations on dp.DonationID equals d.DonationID
                        where d.DonorID == DonorID
                        select new DonationsGrid
                        {
                            DonationAmount = d.DonationAmount,
                            DonationDate = d.DonationDate,
                            PurposeName = p.PurposeName,
                            PurposeDescription = p.PurposeDescription,
                            DonorID = d.DonorID,
                            DonationPurposeID = dp.DonationPurposeID,
                            PurposeID = p.PurposeID,
                            DonationID = d.DonationID
                        };

            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }
    }
}
