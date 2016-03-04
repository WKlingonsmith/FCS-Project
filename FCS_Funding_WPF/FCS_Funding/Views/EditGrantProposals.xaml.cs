﻿using FCS_DataTesting;
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
            OrganizationName = p.OrganizationName;
            GrantProposalID = p.GrantProposalID;
            DonorID = p.DonorID;
            InitializeComponent();
        }

        private void Status_DropDown(object sender, RoutedEventArgs e)
        {
            List<string> p = new List<string>()
            {
                "Pending", "Accepted", "Not Accepted"
            };
            var box = sender as ComboBox;
            box.ItemsSource = p;

        }

        private void Update_Grant_Proposal(object sender, RoutedEventArgs e)
        {
            if (GrantName != "" && GrantName != null)
            {
                string GrantStatus = Status.SelectedValue.ToString();
                MessageBox.Show(GrantStatus + "\n" + "\n" + GrantName + "\n" + OrganizationName);

                Models.FCS_FundingContext db = new Models.FCS_FundingContext();                                
                var grantproposal = (from p in db.GrantProposals
                                     where p.GrantProposalID == GrantProposalID
                                     select p).First();
                grantproposal.GrantName = GrantName;
                if (Status.IsEnabled == true)
                {
                    grantproposal.GrantStatus = GrantStatus;
                    db.SaveChanges();
                    ///OPEN ANOTHER WINDOW TO ADD THIS PROPOSAL TO DONATION & PURPOSE TABLE
                    AddNewGrant adg = new AddNewGrant(DonorID, GrantProposalID);
                    adg.Topmost = true;
                    adg.Show();
                }
                else
                {
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Select a date");
            }
            }
    }
}