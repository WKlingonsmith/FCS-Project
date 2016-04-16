using FCS_DataTesting;
using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateGrant.xaml
    /// </summary>
    public partial class UpdateGrant : Window
    {
        public string GrantName { get; set; }
        public decimal DonationAmount { get; set; }
        public decimal DonationAmountRemaining { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }

        //helper ID's
        public int PurposeID { get; set; }
        public int DonationID { get; set; }
        public int DonorID { get; set; }
        public int GrantProposalID { get; set; }

        public UpdateGrant(GrantsDataGrid g)
        {
            GrantName = g.GrantName;
            DonationAmount = g.DonationAmount;
            DonationAmountRemaining = g.DonationAmountRemaining;
            PurposeName = g.PurposeName;
            PurposeDescription = g.PurposeDescription;
            PurposeID = g.PurposeID;
            DonationID = g.DonationID;
            DonorID = g.DonorID;
            GrantProposalID = g.GrantProposalID;
            InitializeComponent();
        }

        private void Update_Grant(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingDBModel db = new FCS_Funding.Models.FCS_FundingDBModel();
            var purpose = (from p in db.Purposes
                           where p.PurposeID == PurposeID
                           select p).First();
            purpose.PurposeName = PurposeName;
            purpose.PurposeDescription = PurposeDescription;

            var donation = (from d in db.Donations
                         where d.DonationID == DonationID
                         select d).First();
            if(DonAmount.IsEnabled)
            {
                donation.DonationAmount = DonationAmount;
            }
            if(AmountRem.IsEnabled)
            {
                donation.DonationAmountRemaining = DonationAmountRemaining;
            }
            donation.DonationDate = Convert.ToDateTime(DonationDate.ToString());
            donation.DonationExpirationDate = Convert.ToDateTime(DonationExpirationDate.ToString());

            db.SaveChanges();
            MessageBox.Show("Updated these changes successfully.");
            this.Close();

        }

        private void Delete_Grant(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation", 
                "Are you sure that you want to delete this Grant?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingDBModel db = new FCS_Funding.Models.FCS_FundingDBModel();
                var donationPurposes = (from p in db.DonationPurposes
                                    where p.PurposeID == PurposeID
                                    select p);
                foreach (Models.DonationPurpose purp in donationPurposes)
                {
                    db.DonationPurposes.Remove(purp);
                }
                var purpose = (from p in db.Purposes
                               where p.PurposeID == PurposeID
                               select p).First();

                var donation = (from d in db.Donations
                                where d.DonationID == DonationID
                                select d).First();

                var grantProposal = (from d in db.GrantProposals
                                where d.GrantProposalID == GrantProposalID
                                select d).First();
                grantProposal.GrantStatus = "Pending";

                db.Purposes.Remove(purpose);
                db.Donations.Remove(donation);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Grant but the Proposal for this grant has been set to Pending.");
                this.Close();
            }
        }
    }
}
