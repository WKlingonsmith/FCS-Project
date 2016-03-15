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
                && DonationDate.ToString() != "" && DonationExpirationDate.ToString() != "")
            {
                try
                {
                    MessageBox.Show(DonationAmount.ToString() + "\n" + DonationDate + "\n" + 
                        DonationExpirationDate + "\n" + PurposeName + "\n" + PurposeDescription);
                    FCS_FundingContext db = new FCS_FundingContext();
                    Purpose p = new Purpose(PurposeName, PurposeDescription);
                    Donation d = new Donation(DonorID, GrantProposalID, true, false, DonationAmount, Convert.ToDateTime(DonationDate.ToString()),
                        Convert.ToDateTime(DonationExpirationDate.ToString()), DonationAmount);
                    DonationPurpose dp = new DonationPurpose(d.DonationID, p.PurposeID, DonationAmount);
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
