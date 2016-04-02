using FCS_Funding.Models;
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
                    FCS_FundingContext db = new FCS_FundingContext();

                    Purpose p = new Purpose(PurposeName, PurposeDescription);
                    db.Purposes.Add(p);
                    db.SaveChanges();

                    if (IsEvent)
                    {
                        Donation d = new Donation(DonorID, true, false, DonationAmount, Convert.ToDateTime(DonationDate.ToString()), EventID);
                        db.Donations.Add(d);
                        db.SaveChanges();

                        DonationPurpose dp = new DonationPurpose(d.DonationID, p.PurposeID, DonationAmount);
                        db.DonationPurposes.Add(dp);
                        db.SaveChanges();
                    }
                    else
                    {
                        Donation d = new Donation(DonorID, true, false, DonationAmount, Convert.ToDateTime(DonationDate.ToString()));
                        db.Donations.Add(d);
                        db.SaveChanges();

                        DonationPurpose dp = new DonationPurpose(d.DonationID, p.PurposeID, DonationAmount);
                        db.DonationPurposes.Add(dp);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Successfully added Donation");
                    this.Close();
                //}
                //catch (Exception ex)
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
