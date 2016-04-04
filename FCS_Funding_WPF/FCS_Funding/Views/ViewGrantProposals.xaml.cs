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
    /// Interaction logic for ViewGrantProposals.xaml
    /// </summary>
    public partial class ViewGrantProposals : Window
    {
        public string GrantName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime SubmissionDueDate { get; set; }
        public string GrantStatus { get; set; } 
        public string StaffDBRole { get; set; }

        public ViewGrantProposals(string StaffRole)
        {
            StaffDBRole = StaffRole;
            InitializeComponent();
        }

        private void Grant_Proposal_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            var query = from g in db.GrantProposals
                        join d in db.Donors on g.DonorID equals d.DonorID
                        select new GrantProposalGrid
                        {
                            GrantName = g.GrantName,
                            OrganizationName = d.OrganizationName,
                            SubmissionDueDate = g.SubmissionDueDate,
                            GrantStatus = g.GrantStatus,
                            GrantProposalID = g.GrantProposalID,
                            DonorID = g.DonorID
                        };
            // ... Assign ItemsSource of DataGrid. 
            var grid = sender as DataGrid;
            if (grid == null)
            {
                GrantProposals.ItemsSource = query.ToList();
            }
            else
            {
                grid.ItemsSource = query.ToList();
            }
        }
        private void EditGrantProposal(object sender, MouseButtonEventArgs e)
        {
            int Count = Application.Current.Windows.Count;
            if (Count < 3 && StaffDBRole != "Basic")
            {
                int index;
                DataGrid dg = sender as DataGrid;
                GrantProposalGrid p = (GrantProposalGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                if (p.GrantStatus == "Accepted") { index = 1; }
                else if (p.GrantStatus == "Not Accepted") { index = 2; }
                else { index = 0; }

                Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
                EditGrantProposals dgp = new EditGrantProposals(p);
                if (StaffDBRole != "Admin")
                {
                    dgp.Deletegrantprop.IsEnabled = false;
                }
                dgp.Show();
                dgp.Topmost = true;
                dgp.oName.IsEnabled = false;
                if (index == 1 || index == 2)
                {
                    dgp.Status.IsEnabled = false;
                }
                dgp.Status.SelectedIndex = index;
            }
        }
    }
}