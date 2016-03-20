using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateGrantProposal.xaml
    /// </summary>
    public partial class CreateGrantProposal : Window
    {
        public string GrantName { get; set; }

        public CreateGrantProposal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method logs you into the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Organization_DropDown(object sender, RoutedEventArgs e)
        {
            List<string> p = new List<string>()
            {
                "HAFB", "Weber", "Clearfield"
            };
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var query = (from o in db.Donors
                         where o.OrganizationName != null && o.OrganizationName != ""
                         orderby o.OrganizationName
                         select o.OrganizationName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Create_Grant_Proposal(object sender, RoutedEventArgs e)
        {
            if (SubmissionDueDate.ToString() != "" && Organization.SelectedValue != null && GrantName != null && GrantName != "")
            {
                string organiz = Organization.SelectedValue.ToString();
                DateTime datet = Convert.ToDateTime(SubmissionDueDate.ToString());

                MessageBox.Show(organiz + "\n" + datet + "\n" + GrantName + "\n" + "Status is Pending");
                Models.FCS_FundingContext db = new Models.FCS_FundingContext();
                int DonorID = (from d in db.Donors
                               where d.OrganizationName == organiz
                               select d.DonorID).Distinct().First();

                Models.GrantProposal gp = new Models.GrantProposal(DonorID, GrantName, datet, "Pending");
                db.GrantProposals.Add(gp);
                db.SaveChanges();
                MessageBox.Show("Successfully added grant proposal");
                this.Close();
            }
            else
            {
                MessageBox.Show("Select a date");
            }

        }
    }
}
