
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

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateContact.xaml
    /// </summary>
    public partial class UpdateContact : Window
    {
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int DonorID { get; set; }
        public int ContactID { get; set; }
        public UpdateContact(DonorContactGrid dcg)
        {
            ContactFirstName = dcg.ContactFirstName;
            ContactLastName = dcg.ContactLastName;
            ContactPhone = dcg.ContactPhone;
            ContactEmail = dcg.ContactEmail;
            DonorID = dcg.DonorID;
            ContactID = dcg.ContactID;
            InitializeComponent();
        }

        private void Update_Contact(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
            var contact = (from p in db.DonorContacts
                         where p.ContactID == ContactID
                         select p).First();
            contact.ContactFirstName = ContactFirstName;
            contact.ContactLastName = ContactLastName;
            contact.ContactPhone = ContactPhone;
            contact.ContactEmail = ContactEmail;
            if (ContactPhone.Length < 11)
            {
                db.SaveChanges();
                MessageBox.Show("Updated these changes successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("You have entered an invalid phone number. Make sure it is less than or equal to 10 digits.");
            }
        }

        private void Delete_Contact(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation", "Are you sure that you want to delete this Contact?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
                var donorContact = (from p in db.DonorContacts
                                    where p.ContactID == ContactID
                                    select p).First();

                db.DonorContacts.Remove(donorContact);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Patient.");
                this.Close();
            }
        }
    }
}
