using FCS_Funding.Models;
using System.Windows;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AddNewContact.xaml
    /// </summary>
    public partial class AddNewContact : Window
    {
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int DonorID { get; set; }

        public AddNewContact(int donID)
        {
            DonorID = donID;
            InitializeComponent();
        }

        private void Add_Contact(object sender, RoutedEventArgs e)
        {
            if(ContactFirstName != null && ContactFirstName != ""  && ContactLastName != null && ContactLastName != "")
            {
                //MessageBox.Show(ContactFirstName + "\n" + ContactLastName + "\n" + ContactPhone + "\n" + ContactEmail);

                FCS_FundingDBModel db = new FCS_FundingDBModel();
                DonorContact d = new DonorContact();

                d.ContactFirstName = ContactFirstName;
                d.ContactLastName = ContactLastName;
                d.ContactPhone = ContactPhone;
                d.ContactEmail = ContactEmail;
                d.DonorID = DonorID;

                db.DonorContacts.Add(d);
                if (ContactPhone.Length < 11)
                {
                    db.SaveChanges();
                    MessageBox.Show("Successfully added contact");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("You entered an invalid phone number. Make sure it is less than or equal to 10 digits.");
                }
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you input correct data.");
            }

        }
    }
}
