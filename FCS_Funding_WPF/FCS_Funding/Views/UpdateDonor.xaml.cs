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
using FCS_DataTesting;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateDonor.xaml
    /// </summary>
    public partial class UpdateDonor : Window
    {
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorAddress1 { get; set; }
        public string DonorAddress2 { get; set; }
        public string DonorCity { get; set; }
        public string DonorState { get; set; }
        public string DonorType { get; set; }
        public string DonorZip { get; set; }
        public string OrganizationName { get; set; }
        private int DonorID { get; set; }

        public UpdateDonor(DonorsDataGrid d)
        {
            DonorFirstName = d.DonorFirstName;
            DonorLastName = d.DonorLastName;
            DonorAddress1 = d.DonorAddress1;
            DonorAddress2 = d.DonorAddress2;
            DonorCity = d.DonorCity;
            DonorState = d.DonorState;
            DonorType = d.DonorType;
            DonorZip = d.DonorZip;
            DonorID = d.DonorID;
            OrganizationName = d.OrganizationName;
            InitializeComponent();
        }

        private void Update_Donor(object sender, RoutedEventArgs e)
        {
            FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
            var donor = (from p in db.Donors
                           where p.DonorID == DonorID
                           select p).First();
            donor.DonorFirstName = DonorFirstName;
            donor.DonorLastName = DonorLastName;
            donor.DonorAddress1 = DonorAddress1;
            donor.DonorAddress2 = DonorAddress2;
            donor.DonorCity = DonorCity;
            donor.DonorState = DonorState;
            donor.DonorZip = DonorZip;
            donor.OrganizationName = OrganizationName;
            donor.DonorType = DonorType;
            int changes = db.SaveChanges();
            MessageBox.Show("Updated these changes successfully.");
            this.Close();
        }

        private void Delete_Donor(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation", "Are you sure that you want to delete this Patient?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
                var donor = (from p in db.Donors
                               where p.DonorID == DonorID
                               select p).First();
                db.Donors.Remove(donor);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Patient.");
                this.Close();
            }
        }
    }
}
