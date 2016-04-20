using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            Models.FCS_DBModel db = new Models.FCS_DBModel();
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

                //MessageBox.Show(organiz + "\n" + datet + "\n" + GrantName + "\n" + "Status is Pending");
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                int DonorID = (from d in db.Donors
                               where d.OrganizationName == organiz
                               select d.DonorID).Distinct().First();

                Models.GrantProposal gp = new Models.GrantProposal();

                gp.DonorID = DonorID;
                gp.GrantName = GrantName;
                gp.SubmissionDueDate = datet;
                gp.GrantStatus = "Pending";

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
