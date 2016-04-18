using FCS_Funding.Models;
using System;
using System.Windows;

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

        //Helper ID's
        public int DonorID { get; set; }

        public CreateMoneyDonation(int donorID, bool isEvent, int eventID)
        {
            EventID = eventID;
            IsEvent = isEvent;
            DonorID = donorID;
            InitializeComponent();
        }

        private void AddGrant(object sender, RoutedEventArgs e)
        {
            if (DonationAmount != 0 && PurposeName != null && PurposeName != "" && PurposeDescription != null && PurposeDescription != ""
                && DonationDate.ToString() != "")
            {
                //try
                //{
                MessageBox.Show(DonationAmount.ToString() + "\n" + DonationDate + "\n" +
                    PurposeName + "\n" + PurposeDescription);
                FCS_FundingDBModel db = new FCS_FundingDBModel();

                Purpose p = new Purpose();
                p.PurposeName = PurposeName;
                p.PurposeDescription = PurposeDescription;
                db.Purposes.Add(p);
                db.SaveChanges();

                if (IsEvent)
                {
                    Donation d = new Donation();
                    d.DonorID = DonorID;
                    d.Restricted = true;
                    d.InKind = false;
                    d.DonationAmount = DonationAmount;
                    d.DonationDate = Convert.ToDateTime(DonationDate.ToString());
                    d.EventID = EventID;
                    db.Donations.Add(d);
                    db.SaveChanges();

                    DonationPurpose dp = new DonationPurpose();
                    dp.DonationID = d.DonationID;
                    dp.PurposeID = p.PurposeID;
                    dp.DonationPurposeAmount = DonationAmount;
                    db.DonationPurposes.Add(dp);
                    db.SaveChanges();
                }
                else
                {
                    Donation d = new Donation();
                    d.DonorID = DonorID;
                    d.Restricted = true;
                    d.InKind = false;
                    d.DonationAmount = DonationAmount;
                    d.DonationDate = Convert.ToDateTime(DonationDate.ToString());

                    db.Donations.Add(d);
                    db.SaveChanges();

                    DonationPurpose dp = new DonationPurpose();
                    dp.DonationID = d.DonationID;
                    dp.PurposeID = p.PurposeID;
                    dp.DonationPurposeAmount = DonationAmount;
                    db.DonationPurposes.Add(dp);
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
            else
            {
                MessageBox.Show("Make sure to input all the correct data.");
            }
        }
    }
}
