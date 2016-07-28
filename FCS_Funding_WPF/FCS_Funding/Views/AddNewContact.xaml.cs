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
			text_FirstName.Focus();
        }

        private void Add_Contact(object sender, RoutedEventArgs e)
        {
            if(ContactFirstName != null && ContactFirstName != ""  && ContactLastName != null && ContactLastName != "")
            {

                FCS_DBModel db = new FCS_DBModel();
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
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The phone number entered is invalid.");
                }
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Please check the data entered.");
            }

        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
