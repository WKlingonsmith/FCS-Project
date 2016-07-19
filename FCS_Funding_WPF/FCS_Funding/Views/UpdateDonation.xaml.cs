using FCS_DataTesting;
using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using FCS_Funding.Models;
using System.Collections.Generic;
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
    /// Interaction logic for UpdateDonation.xaml
    /// </summary>
    public partial class UpdateDonation : Window
    {
        public decimal DonationAmount { get; set; }
        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int PurposeID { get; set; }
        public int DonationPurposeID { get; set; }
        public List<string> purpose = new List<string>();
        public UpdateDonation(DonationsGrid donation)
        {
            FCS_DBModel db = new FCS_DBModel();
            foreach (var item in db.Purposes)
            {
                purpose.Add(item.PurposeName);
            }
            DataContext = purpose;

            DonationAmount = donation.DonationAmount;
            DonorID = donation.DonorID;
            DonationPurposeID = donation.DonationPurposeID;
            PurposeID = donation.PurposeID;
            DonationID = donation.DonationID;

            InitializeComponent();

			text_DonationAmount.Focus();

            var restricted = (from item in db.Donations where item.DonationID == DonationID select item.Restricted).First();
            var grantID = (from item in db.Donations where item.DonationID == DonationID select item.GrantProposalID).First();
            var grantDate = (from item in db.Donations where item.DonationID == DonationID select item.DonationDate).First();
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
                DonationExpiration.SelectedDate = donationTable.DonationExpirationDate;
                restrictedCheckBox.IsChecked = true;
                PurposeComboBox.SelectedItem = (from p in db.Purposes
                                                join dp in db.DonationPurposes
                                                on p.PurposeID equals dp.PurposeID
                                                join don in db.Donations
                                                on dp.DonationID equals don.DonationID
                                                select p.PurposeName).First();
            }

        }

        public void UpdateThisDonation(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
            //var purpose = (from p in db.Purposes
            //               where p.PurposeID == PurposeID
            //               select p).First();
            //purpose.PurposeName = PurposeName;
            //purpose.PurposeDescription = PurposeDescription;

            var donation = (from d in db.Donations
                            where d.DonationID == DonationID
                            select d).First();
            donation.DonationDate = Convert.ToDateTime(DonationDate.ToString());
            if (DonationAmount - donation.DonationAmount < 0)
            {
                MessageBox.Show("This change would result in a\nnegative balance for this donation");
            }
            else
            {
                if (restrictedCheckBox.IsChecked == true)
                {
                    if (PurposeComboBox.Text != "" && PurposeComboBox.Text != null)
                    {
                        DeletePurposes delPurp = new DeletePurposes();
                        delPurp.deletePurpose(DonationID);
                        Purpose p = new Purpose();
                        DonationPurpose dp = new DonationPurpose();
                        string purposeName = PurposeComboBox.SelectedItem.ToString();
                        int PurposeID = db.Purposes.Where(x => x.PurposeName == purposeName).Select(x => x.PurposeID).First();
                        donation.Restricted = true;
                        if (DonationExpiration != null && DonationExpiration.ToString() != "")
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
                        decimal donationDiff = donation.DonationAmount - DonationAmount;
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
                    decimal donationDiff = donation.DonationAmount - DonationAmount;
                    donation.DonationAmount = DonationAmount;
                    donation.DonationAmountRemaining = donation.DonationAmountRemaining - donationDiff;
                    db.Entry(donation);
                    db.SaveChanges();
                }

                MessageBox.Show("Updated these changes successfully.");
                this.Close();
            }
        }



        private void DeleteDonation(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Donation?",
                 "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
                DeletePurposes delPurp = new DeletePurposes();

                delPurp.deletePurpose(DonationID);

                //var purpose = (from p in db.Purposes
                //               where p.PurposeID == PurposeID
                //               select p).First();

                var donation = (from d in db.Donations
                                where d.DonationID == DonationID
                                select d).First();

                //db.Purposes.Remove(purpose);
                db.Donations.Remove(donation);
                db.SaveChanges();
                MessageBox.Show("Donation Deleted.");
                this.Close();
            }

        }
        private void restrictedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (restrictedCheckBox.IsChecked == true)
            {
                try
                {
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

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
