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
    /// Interaction logic for UpdateDonation.xaml
    /// </summary>
    public partial class UpdateDonation : Window
    {
        public decimal DonationAmount { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }

        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int PurposeID { get; set; }
        public int DonationPurposeID { get; set; }
        public UpdateDonation(DonationsGrid donation)
        {
            DonationAmount = donation.DonationAmount;
            PurposeName = donation.PurposeName;
            PurposeDescription = donation.PurposeDescription;
            DonorID = donation.DonorID;
            DonationPurposeID = donation.DonationPurposeID;
            PurposeID = donation.PurposeID;
            DonationID = donation.DonationID;

            InitializeComponent();
        }

        private void UpdateThisDonation(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
            var purpose = (from p in db.Purposes
                           where p.PurposeID == PurposeID
                           select p).First();
            purpose.PurposeName = PurposeName;
            purpose.PurposeDescription = PurposeDescription;

            var donation = (from d in db.Donations
                            where d.DonationID == DonationID
                            select d).First();
            donation.DonationAmount = DonationAmount;
            donation.DonationDate = Convert.ToDateTime(DonationDate.ToString());

            db.SaveChanges();
            MessageBox.Show("Updated these changes successfully.");
            this.Close();

        }

        private void DeleteDonation(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Are you sure that you want to delete this Donation?" ,
                 "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
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

                db.Purposes.Remove(purpose);
                db.Donations.Remove(donation);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Doation.");
                this.Close();
            }

        }
    }
}
