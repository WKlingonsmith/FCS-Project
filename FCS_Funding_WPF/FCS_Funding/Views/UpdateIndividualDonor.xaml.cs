
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

        public UpdateIndividualDonor(DonorsDataGrid p, Models.DonorContact d)
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

            InitializeComponent();
        }

        private void UpdateDonor(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
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
            MessageBox.Show("You successfully updated this Donor");
            this.Close();
        }

        private void DeleteDonor(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation", "Are you sure that you want to delete this Donor?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
                var donor = (from d in db.Donors
                             where d.DonorID == DonorID
                             select d).First();
                var contact = (from c in db.DonorContacts
                               where c.ContactID == ContactID
                               select c).First();
                db.DonorContacts.Remove(contact);
                db.Donors.Remove(donor);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Donor.");
                this.Close();
            }
        }
    }
}