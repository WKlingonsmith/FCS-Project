using FCS_DataTesting;
using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public List<string> purpose = new List<string>();
        public UpdateGrant(GrantsDataGrid g)
        {
            FCS_DBModel db = new FCS_DBModel();
            Donation d = new Donation();
            foreach (var item in db.Purposes)
            {
                purpose.Add(item.PurposeName);
            }
            DataContext = purpose;

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
            try
            {
                var restricted = (from item in db.Donations where item.DonationID == DonationID select item.Restricted).First();

                if (restricted == true)
                {
                    var donationTable = (from don in db.Donations
                                         join dp in db.DonationPurposes
                                         on don.DonationID equals dp.DonationID
                                         join p in db.Purposes
                                         on dp.PurposeID equals p.PurposeID
                                         where don.DonationID == DonationID
                                         select new
                                         {
                                             don.DonationID,
                                             don.DonationExpirationDate,
                                             don.Restricted,
                                             dp.PurposeID
                                         }).First();
                    DonationDate.IsEnabled = false;
                    try {
                        DonationExpiration.SelectedDate = donationTable.DonationExpirationDate;
                    }
                    catch
                    {

                    }
                    restrictedCheckBox.IsChecked = true;
                    PurposeComboBox.SelectedItem = (from p in db.Purposes
                                                    join dp in db.DonationPurposes
                                                    on p.PurposeID equals dp.PurposeID
                                                    join don in db.Donations
                                                    on dp.DonationID equals don.DonationID
                                                    select p.PurposeName).First();

                    //DonationExpiration.IsEnabled = false;
                    //restrictedCheckBox.IsEnabled = false;
                    //PurposeComboBox.IsEnabled = false;
                }
            }
            catch
            {

            }
        }

        private void Update_Grant(object sender, RoutedEventArgs e)
        {
            DonationPurpose dp = new DonationPurpose();
            FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
            //var purpose = (from p in db.Purposes
            //               where p.PurposeID == PurposeID
            //               select p).First();
            //purpose.PurposeName = PurposeName;
            //purpose.PurposeDescription = PurposeDescription;
            decimal donationDiff = 0m;
            var donation = (from d in db.Donations
                         where d.DonationID == DonationID
                         select d).First();
            if(DonAmount.IsEnabled)
            {
                donationDiff = DonationAmount - donation.DonationAmount;
                donation.DonationAmount = DonationAmount;
                
            }
            if (Convert.ToDecimal(AmountRem.Text) > 0)
            {
                donation.DonationAmountRemaining = DonationAmountRemaining + donationDiff;
            }
            if (DonationDate.ToString() != null && DonationDate.ToString() != "")
            {
                donation.DonationDate = Convert.ToDateTime(DonationDate.ToString());
                try {
                    donation.DonationExpirationDate = Convert.ToDateTime(DonationExpiration.ToString());
                }
                catch
                {
                    donation.DonationExpirationDate = null;
                }
            }
            donation.DonationDate = Convert.ToDateTime(DonationDate.ToString());
            if (restrictedCheckBox.IsChecked == true)
            {
                if (PurposeComboBox.Text != "" && PurposeComboBox.Text != null)
                {
                    DeletePurposes delPurp = new DeletePurposes();

                    delPurp.deletePurpose(DonationID);

                    Purpose p = new Purpose();
                    dp = new DonationPurpose();
                    string purposeName = PurposeComboBox.SelectedItem.ToString();
                    int PurposeID = db.Purposes.Where(x => x.PurposeName == purposeName).Select(x => x.PurposeID).First();
                    donation.Restricted = true;

                    if (DonationExpiration.ToString() != null && DonationExpiration.ToString() != "")
                    {
                        donation.DonationExpirationDate = Convert.ToDateTime(DonationExpiration.ToString());
                    }
                    else {
                        donation.DonationExpirationDate = null;
                    }
                    dp.DonationID = donation.DonationID;
                    dp.PurposeID = PurposeID;
                    dp.DonationPurposeAmount = DonationAmount;
                    db.DonationPurposes.Add(dp);
                    donationDiff = donation.DonationAmount - DonationAmount;
                    donation.DonationAmount = DonationAmount;
                    donation.DonationAmountRemaining = donation.DonationAmountRemaining - donationDiff;
                    db.Entry(donation);
                    db.SaveChanges();
                    db.Entry(dp);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Please enter a purpose if the donation is restrcted");
                }
            }
            else
            {
                DeletePurposes delPurp = new DeletePurposes();
                delPurp.deletePurpose(DonationID);
                donation.Restricted = false;
                donation.DonationExpirationDate = null;
                donationDiff = donation.DonationAmount - DonationAmount;
                donation.DonationAmount = DonationAmount;
                donation.DonationAmountRemaining = donation.DonationAmountRemaining - donationDiff;
                db.Entry(donation);
                db.SaveChanges();
            }
        

            //if (restrictedCheckBox.IsChecked == true)
            //{
            //    Purpose p = new Purpose();

            //    string purposeName = PurposeComboBox.SelectedItem.ToString();
            //    int PurposeID = db.Purposes.Where(x => x.PurposeName == purposeName).Select(x => x.PurposeID).First();

            //    donation.Restricted = true;
            //    if(DonationExpiration.ToString() != null && DonationExpiration.ToString() != "") {
            //        donation.DonationExpirationDate = Convert.ToDateTime(DonationExpiration.ToString());
            //    }
            //    dp.DonationID = donation.DonationID;
            //    dp.PurposeID = PurposeID;
            //    dp.DonationPurposeAmount = DonationAmount;
            //    db.DonationPurposes.Add(dp);
            //    //db.Donations.Remove(donation);                
            //}
            //else
            //{
            //    donation.Restricted = false;
            //    donation.DonationExpirationDate = null;
            //    if (dp.PurposeID > 0)
            //    {
            //        dp.DonationPurposeID = 0;
            //        dp.PurposeID = 0;
            //    }
            //    dp.DonationPurposeAmount = 0m;

            //}
            if (donation.DonationAmountRemaining > 0)
            {
                db.Donations.Add(donation);
                db.Entry(donation).State = EntityState.Modified;
                db.SaveChanges();
                MessageBox.Show("Updated these changes successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("This would result in a negative balance for this grant.");
            }

        }

        private void Delete_Grant(object sender, RoutedEventArgs e)
        {
            FCS_DBModel db = new FCS_DBModel();
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Grant?", 
                 "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                var donationPurposes = (from p in db.DonationPurposes
                                    where p.PurposeID == PurposeID
                                    select p);
                DeletePurposes delPurp = new DeletePurposes();

                    delPurp.deletePurpose(DonationID);

                }
                //var purpose = (from p in db.Purposes
                //               where p.PurposeID == PurposeID
                //               select p).First();


                var donation = (from d in db.Donations
                                where d.DonationID == DonationID
                                select d).First();
                //try {
                //    var donationPurpose = (from dp in db.DonationPurposes
                //                           where dp.DonationID == donation.DonationID
                //                           select dp);
                //    foreach (var item in donationPurpose)
                //    {
                //        db.DonationPurposes.Remove(item);
                //        db.SaveChanges();
                //    }
                //}
                //catch
                //{

                //}
                var grantProposal = (from d in db.GrantProposals
                                where d.GrantProposalID == GrantProposalID
                                select d).First();
                grantProposal.GrantStatus = "Pending";

                db.Donations.Remove(donation);
                db.SaveChanges();
                MessageBox.Show("This grant has been deleted and its associated proposal has been set to Pending.");
                this.Close();
            }
        
        private void restrictedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (restrictedCheckBox.IsChecked == true)
            {
                try {
                    PurposeComboBox.IsEnabled = true;
                }
                catch
                {

                }
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
