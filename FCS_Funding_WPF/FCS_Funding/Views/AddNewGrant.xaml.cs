using System;
using System.Windows;
using FCS_Funding.Models;

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
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }
        
        //Helper ID's
        public int DonorID { get; set; }
        public int GrantProposalID { get; set; }

        public AddNewGrant(int dID, int gpID)
        {
            DonorID = dID;
            GrantProposalID = gpID;
            InitializeComponent();
        }

        private void AddGrant(object sender, RoutedEventArgs e)
        {
            if (DonationAmount != 0 && PurposeName != null && PurposeName != "" && PurposeDescription != null && PurposeDescription != "" 
                && DonationDate.ToString() != "")
            {
                try
                {
                    //MessageBox.Show(DonationAmount.ToString() + "\n" + DonationDate + "\n" + 
                    //    DonationExpirationDate + "\n" + PurposeName + "\n" + PurposeDescription);
                    FCS_DBModel db = new FCS_DBModel();
                    Purpose p = new Purpose();
                    p.PurposeName = PurposeName;
                    p.PurposeDescription = PurposeDescription;

                    Donation d = new Donation();
                    d.DonorID = DonorID;
                    d.GrantProposalID = GrantProposalID;
                    d.Restricted = true;
                    d.InKind = false;
                    d.DonationAmount = DonationAmount;
                    d.DonationAmountRemaining = DonationAmount;
                    d.DonationDate = Convert.ToDateTime(DonationDate.ToString());
                    try
                    {
                        d.DonationExpirationDate = Convert.ToDateTime(DonationExpirationDate.ToString());
                    }
                    catch {
                    }
                    d.DonationAmount = DonationAmount;

                    DonationPurpose dp = new DonationPurpose();
                    dp.DonationID = d.DonationID;
                    dp.PurposeID = p.PurposeID;
                    dp.DonationPurposeAmount = DonationAmount;

                    db.Donations.Add(d);
                    db.Purposes.Add(p);

                    db.DonationPurposes.Add(dp);
                    db.SaveChanges();
                    MessageBox.Show("Successfully added Grant");
                    this.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Cannot add Grant" + "\n" + ex);
                }
            }
            else
            {
                MessageBox.Show("Make sure to input all the correct data.");
            }
        }
    }
}
