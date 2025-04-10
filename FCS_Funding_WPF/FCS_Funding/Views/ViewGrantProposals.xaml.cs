﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FCS_DataTesting;

namespace FCS_Funding.Views
{
	using Definition;
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
        private void Refresh_Grant_Proposal(object sender, RoutedEventArgs e)
        {
            sender = GrantProposals;
            Grant_Proposal_Grid(sender, e);
        }
        private void Grant_Proposal_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
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
            int index;
            DataGrid dg = sender as DataGrid;
            if (dg.SelectedIndex != -1)
            {
                GrantProposalGrid p = (GrantProposalGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                if (p.GrantStatus == "Accepted") { index = 1; }
                else if (p.GrantStatus == "Not Accepted") { index = 2; }
                else { index = 0; }

                Models.FCS_DBModel db = new Models.FCS_DBModel();
                EditGrantProposals dgp = new EditGrantProposals(p);
                dgp.ShowDialog();
                dgp.text_GrantName.IsEnabled = false;
                if (index == 1 || index == 2)
                {
                    dgp.combobox_Status.IsEnabled = false;
                }
                dgp.combobox_Status.SelectedIndex = index;
            }
            Refresh_Grant_Proposal(sender, e);
        }


        private void Close_Grants(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}