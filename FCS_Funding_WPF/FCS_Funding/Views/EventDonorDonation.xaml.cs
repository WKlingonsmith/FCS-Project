using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for EventDonorDonation.xaml
    /// </summary>
    public partial class EventDonorDonation : Window
    {
        public int EventID { get; set; }
        public EventDonorDonation(int eventID)
        {
            EventID = eventID;
            InitializeComponent();
			OrgOrIndividual.Focus();
        }

        private void Individual_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var query = (from o in db.Donors
                         join c in db.DonorContacts on o.DonorID equals c.DonorID
                         where o.DonorType == "Individual" || o.DonorType == "Anonymous"
                         orderby c.ContactLastName
                         select c.ContactFirstName + ", " + c.ContactLastName + ", " + c.ContactPhone).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Organization_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var query = (from o in db.Donors
                         where o.DonorType == "Organization"
                         orderby o.OrganizationName
                         select o.OrganizationName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Change_Organization_Individual(object sender, RoutedEventArgs e)
        {
            if (OrgOrIndividual.IsChecked.Value)
            {
                Individual.IsEnabled = false;
                Organization.IsEnabled = true;
            }
            else
            {
                Individual.IsEnabled = true;
                Organization.IsEnabled = false;
            }
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            //Then its an organization
            if (OrgOrIndividual.IsChecked.Value && Organization.SelectedIndex != -1)
            {
                string Organiz = Organization.SelectedValue.ToString();
                var donorID = (from d in db.Donors
                                where d.OrganizationName == Organiz
                                select d.DonorID).Distinct().First();

                CreateMoneyDonation cmd = new CreateMoneyDonation(donorID, true, EventID);
                cmd.ShowDialog();
                this.Close();

            }
            //then its an individual
            else if (Individual.SelectedIndex != -1)
            {
                string[] separators = new string[] { ", " };
                string Indiv = Individual.SelectedValue.ToString();
                string[] words = Indiv.Split(separators, StringSplitOptions.None);
                string FName = words[0]; string LName = words[1]; string FNumber = words[2];
                var donorID = (from dc in db.DonorContacts
                               join d in db.Donors on dc.DonorID equals d.DonorID
                               where dc.ContactFirstName == FName && dc.ContactLastName == LName && dc.ContactPhone == FNumber
                               && (d.DonorType == "Individual" || d.DonorType == "Anonymous")
                               select dc.DonorID).Distinct().FirstOrDefault();

                CreateMoneyDonation cmd = new CreateMoneyDonation(donorID, true, EventID);
                cmd.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Make sure to select an organization or an individual");
                return;
            }

		}

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
