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
        public string GrantName { get; set; }
        public decimal DonationAmount { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }

        public AddNewGrant()
        {
            InitializeComponent();
        }

        private void AddGrant(object sender, RoutedEventArgs e)
        {
            if(GrantName != null && GrantName != "" && DonationAmount != 0 && PurposeName != null && 
                PurposeName != "" && PurposeDescription != null && PurposeDescription != "" && DonationDate != null)
            {
                try
                {
                    MessageBox.Show(GrantName + "\n" + DonationAmount.ToString() + "\n" + DonationDate + "\n" + PurposeName + "\n" + PurposeDescription);
                    FCS_FundingContext db = new FCS_FundingContext();
                    Purpose p = new Purpose(PurposeName, PurposeDescription);
                    DonationPurpose dp = new DonationPurpose();
                    Donation d = new Donation();
                    Donor don = new Donor();
                    GrantProposal gp = new GrantProposal();
                    //db.Purposes.Add(p);
                    //db.SaveChanges();

                    //db.Patients.Add(pat);
                    //db.SaveChanges();
                    this.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Cannot add Grant");
                }
            }
            else
            {
                MessageBox.Show("Make sure to input all the correct data.");
            }
        }
    }
}
