//using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using FCS_DataTesting;
namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for EditGrantProposals.xaml
    /// </summary>
    public partial class EditGrantProposals : Window
    {
        public string GrantName { get; set; }
        public string OrganizationName { get; set; }
        public int GrantProposalID { get; set; }
        public int DonorID { get; set; }

        public EditGrantProposals(GrantProposalGrid p)
        {
            GrantName = p.GrantName;
            GrantProposalID = p.GrantProposalID;
            DonorID = p.DonorID;

			InitializeComponent();

			IEnumerable<string> statusItems = new List<string>() { "Pending", "Accepted", "Not Accepted" };
			combobox_Status.ItemsSource = statusItems;

			Models.FCS_DBModel db = new Models.FCS_DBModel();
			var query = (from o in db.Donors
						 where o.DonorType == "Organization" || o.DonorType == "Government"
						 orderby o.OrganizationName
						 select o.OrganizationName).ToList();
			combobox_Organization.ItemsSource = query;

			combobox_Status.Text = p.GrantStatus;
			combobox_Organization.Text = p.OrganizationName;
			text_GrantName.Focus();
        }

		private void Update_Grant_Proposal(object sender, RoutedEventArgs e)
        {
			if (string.IsNullOrEmpty(text_GrantName.Text))
			{
				MessageBox.Show("Please enter a name for the grant.");
				return;
			}

			if (combobox_Organization.Text == "")
			{
				MessageBox.Show("Please select an organization.");
			}

            string GrantStatus = combobox_Status.SelectedValue.ToString();
            //MessageBox.Show(GrantStatus + "\n" + "\n" + GrantName + "\n" + OrganizationName);

            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var grantproposal = (from p in db.GrantProposals
                                    where p.GrantProposalID == GrantProposalID
                                    select p).First();
            grantproposal.GrantName = GrantName;
            if (combobox_Status.IsEnabled == true)
            {

                grantproposal.GrantStatus = GrantStatus;
                db.SaveChanges();
                if (GrantStatus == "Accepted")
                {
                    ///OPEN ANOTHER WINDOW TO ADD THIS PROPOSAL TO DONATION & PURPOSE TABLE
                    AddNewGrant adg = new AddNewGrant(DonorID, GrantProposalID);
                    adg.ShowDialog();
                }
                this.Close();
            }
            else
            {
                db.SaveChanges();
            }
        }

        private void Delete_Grant_Proposal(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Grant Proposal?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
                var grantproposal = (from p in db.GrantProposals
                                     where p.GrantProposalID == GrantProposalID
                                     select p).First();

                db.GrantProposals.Remove(grantproposal);
                db.SaveChanges();
                MessageBox.Show("This Grant Proposal has been deleted.");
                this.Close();
            }
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
