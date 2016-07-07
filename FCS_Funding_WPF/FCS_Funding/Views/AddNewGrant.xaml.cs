using System;
using System.Windows;
using FCS_Funding.Models;
using System.Linq;
using System.Collections.Generic;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AddNewGrant.xaml
    /// </summary>
    public partial class AddNewGrant : Window
    {
        /// <summary>
        /// Needed properties
        /// </summary>
        public decimal DonationAmount { get; set; }

        //Helper ID's
        public int DonorID { get; set; }
        public int GrantProposalID { get; set; }
        public List<string> purpose = new List<string>();
        public AddNewGrant(int dID, int gpID)
        {
            FCS_DBModel db = new FCS_DBModel();

            foreach (var item in db.Purposes)
            {
                purpose.Add(item.PurposeName);
            }
            DataContext = purpose;
            DonorID = dID;
            GrantProposalID = gpID;
            InitializeComponent();
        }

        private void AddGrant(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBox.Show(DonationAmount.ToString() + "\n" + DonationDate + "\n" +
                //    PurposeName + "\n" + PurposeDescription);
                FCS_DBModel db = new FCS_DBModel();

                Donation d = new Donation();
                d.DonorID = DonorID;
                d.Restricted = false;
                d.InKind = false;
                d.DonationAmount = DonationAmount;
                d.DonationDate = Convert.ToDateTime(DonationDate.ToString());
                d.DonationAmountRemaining = DonationAmount;
                d.GrantProposalID = GrantProposalID;
                db.Donations.Add(d);

                if (restrictedCheckBox.IsChecked == true)
                {
                    Purpose p = new Purpose();
                    DonationPurpose dp = new DonationPurpose();
                    string purposeName = PurposeComboBox.SelectedValue.ToString();
                    int PurposeID = db.Purposes.Where(x => x.PurposeName == purposeName).Select(x => x.PurposeID).First();

                    d.Restricted = true;
                    d.DonationExpirationDate = Convert.ToDateTime(DonationExpiration.ToString());
                    d.GrantProposalID = GrantProposalID;
                    dp.DonationID = d.DonationID;
                    dp.PurposeID = PurposeID;
                    dp.DonationPurposeAmount = DonationAmount;
                    db.DonationPurposes.Add(dp);
                    db.Donations.Remove(d);
                    db.Donations.Add(d);
                }
                db.SaveChanges();
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot add Grant" + "\n" + ex);
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
