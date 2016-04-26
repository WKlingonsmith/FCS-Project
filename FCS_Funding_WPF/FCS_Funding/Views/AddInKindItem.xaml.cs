using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AddInKindItem.xaml
    /// </summary>
    public partial class AddInKindItem : Window
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public bool IsEvent { get; set; }
        public int EventID { get; set; }

        public AddInKindItem(bool isEvent, int eventID)
        {
            EventID = eventID;
            IsEvent = isEvent;
            InitializeComponent();
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
                         where o.OrganizationName != null && o.OrganizationName != ""
                         orderby o.OrganizationName
                         select o.OrganizationName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Add_InKind_Item(object sender, RoutedEventArgs e)
        {
            if (ItemName != null && ItemName != "" && ItemDescription != null && ItemDescription != "")
            {
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                //Then its an organization
                if (OrgOrIndividual.IsChecked.Value && Organization.SelectedIndex != -1)
                {
                    string Organiz = Organization.SelectedValue.ToString();
                    //MessageBox.Show(ItemName + "\n" + ItemDescription + "\n" + DateRecieved + "\n" + Organiz + "\n" + "You got HERE");
                    var donorID = (from d in db.Donors
                                   where d.OrganizationName == Organiz
                                   select d.DonorID).Distinct().First();
                    //MessageBox.Show(donorID.ToString());

                    if (IsEvent)
                    {
                        Models.Donation donation = new Models.Donation();
                        donation.DonorID = donorID;
                        donation.Restricted = false;
                        donation.InKind = true;
                        donation.DonationAmount = 0M;
                        donation.DonationDate = Convert.ToDateTime(DateRecieved.ToString());
                        donation.EventID = EventID;
                        db.Donations.Add(donation);
                        db.SaveChanges();

                        Models.In_Kind_Item inKind = new Models.In_Kind_Item();
                        inKind.DonationID = donation.DonationID;
                        inKind.ItemName = ItemName;
                        inKind.ItemDescription = ItemDescription;
                        db.In_Kind_Item.Add(inKind);
                        db.SaveChanges();
                    }
                    else
                    {
                        Models.Donation donation = new Models.Donation();
                        donation.DonorID = donorID;
                        donation.Restricted = false;
                        donation.InKind = true;
                        donation.DonationAmount = 0M;
                        donation.DonationDate = Convert.ToDateTime(DateRecieved.ToString());
                        db.Donations.Add(donation);
                        db.SaveChanges();

                        Models.In_Kind_Item inKind = new Models.In_Kind_Item();
                        inKind.DonationID = donation.DonationID;
                        inKind.ItemName = ItemName;
                        inKind.ItemDescription = ItemDescription;
                        db.In_Kind_Item.Add(inKind);
                        db.SaveChanges();
                    }
                }
                //then its an individual
                else if (Individual.SelectedIndex != -1)
                {
                    string[] separators = new string[] { ", " };
                    string Indiv = Individual.SelectedValue.ToString();
                    //MessageBox.Show(ItemName + "\n" + ItemDescription + "\n" + DateRecieved + "\n" + Indiv);
                    string[] words = Indiv.Split(separators, StringSplitOptions.None);
                    string FName = words[0]; string LName = words[1]; string FNumber = words[2];
                    var donorID = (from dc in db.DonorContacts
                                   join d in db.Donors on dc.DonorID equals d.DonorID
                                   where dc.ContactFirstName == FName && dc.ContactLastName == LName && dc.ContactPhone == FNumber
                                   && (d.DonorType == "Individual" || d.DonorType == "Anonymous")
                                   select dc.DonorID).Distinct().FirstOrDefault();

                    if (IsEvent)
                    {
                        Models.Donation donation = new Models.Donation();
                        donation.DonorID = donorID;
                        donation.Restricted = false;
                        donation.InKind = true;
                        donation.DonationAmount = 0M;
                        donation.DonationDate = Convert.ToDateTime(DateRecieved.ToString());
                        donation.EventID = EventID;
                        db.Donations.Add(donation);
                        db.SaveChanges();

                        Models.In_Kind_Item inKind = new Models.In_Kind_Item();
                        inKind.DonationID = donation.DonationID;
                        inKind.ItemName = ItemName;
                        inKind.ItemDescription = ItemDescription;
                        db.In_Kind_Item.Add(inKind);
                        db.SaveChanges();
                    }
                    else
                    {
                        Models.Donation donation = new Models.Donation();
                        donation.DonorID = donorID;
                        donation.Restricted = false;
                        donation.InKind = true;
                        donation.DonationAmount = 0M;
                        donation.DonationDate = Convert.ToDateTime(DateRecieved.ToString());
                        db.Donations.Add(donation);
                        db.SaveChanges();

                        Models.In_Kind_Item inKind = new Models.In_Kind_Item();
                        inKind.DonationID = donation.DonationID;
                        inKind.ItemName = ItemName;
                        inKind.ItemDescription = ItemDescription;
                        db.In_Kind_Item.Add(inKind);
                        db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Make sure to select an organization or an individual");
                    return;
                }
                MessageBox.Show("Successfully added In_Kind Item");
                this.Close();

            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure the data is correct.");
            }
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

        private void InKindClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}