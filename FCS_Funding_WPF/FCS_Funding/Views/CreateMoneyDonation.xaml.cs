using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Linq;
namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateMoneyDonation.xaml
    /// </summary>
    public partial class CreateMoneyDonation : Window
    {
        public decimal DonationAmount { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }
        public bool IsEvent { get; set; }
        public int EventID { get; set; }
        public List<string> purpose = new List<string>();

        //Helper ID's
        public int DonorID { get; set; }

        public CreateMoneyDonation(int donorID, bool isEvent, int eventID)
        {
            FCS_DBModel db = new FCS_DBModel();

            foreach (var item in db.Purposes)
            {
                purpose.Add(item.PurposeName);
            }
            DataContext = purpose;

            EventID = eventID;
            IsEvent = isEvent;
            DonorID = donorID;
            InitializeComponent();
        }

        private void AddGrant(object sender, RoutedEventArgs e)
        {
            //if (DonationAmount != 0 && PurposeName != null && PurposeName != "" && DonationDate.ToString() != "")
            //{
            try
            {
                //MessageBox.Show(DonationAmount.ToString() + "\n" + DonationDate + "\n" +
                //    PurposeName + "\n" + PurposeDescription);
                FCS_DBModel db = new FCS_DBModel();

                if (IsEvent)
                {
                    Donation d = new Donation();
                    d.DonorID = DonorID;
                    d.Restricted = true;
                    d.InKind = false;
                    d.DonationAmount = DonationAmount;
                    d.DonationDate = Convert.ToDateTime(DonationDate.ToString());
                    d.EventID = EventID;
                    d.DonationAmountRemaining = DonationAmount;
                    db.Donations.Add(d);
                    db.SaveChanges();

                }
                else
                {
                    Donation d = new Donation();
                    d.DonorID = DonorID;
                    d.Restricted = false;
                    d.InKind = false;
                    d.DonationAmount = DonationAmount;
                    d.DonationDate = Convert.ToDateTime(DonationDate.ToString());
                    d.DonationAmountRemaining = DonationAmount;
                    db.Donations.Add(d);

                    if (restrictedCheckBox.IsChecked == true)
                    {
                        Purpose p = new Purpose();
                        DonationPurpose dp = new DonationPurpose();
                        string purposeName = PurposeComboBox.SelectedItem.ToString();
                        int PurposeID = db.Purposes.Where(x => x.PurposeName == purposeName).Select(x => x.PurposeID).First();

                        d.Restricted = true;
                        d.DonationExpirationDate = Convert.ToDateTime(DonationExpiration.ToString());
                        dp.DonationID = d.DonationID;
                        dp.PurposeID = PurposeID;
                        dp.DonationPurposeAmount = DonationAmount;
                        db.DonationPurposes.Add(dp);
                        db.Donations.Remove(d);
                        db.Donations.Add(d);
                        //db.Entry(d);
                        //db.SaveChanges();
                    }
                    db.SaveChanges();
                }
                MessageBox.Show("Successfully added Donation");
                this.Close();
                //}
                //catch ()
                //{
                //    MessageBox.Show("Cannot add Grant" + "\n" + ex);
                //}
            }
            catch
            {
                MessageBox.Show("Make sure to input all the correct data.");
            }
        }

        private void restrictedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (restrictedCheckBox.IsChecked == true)
            {

                PurposeComboBox.IsEnabled = true;
                DonationExpiration.IsEnabled = true;

            }
            else
            {
                PurposeComboBox.IsEnabled = false;
                DonationExpiration.IsEnabled = false;
            }
        }
    }
}