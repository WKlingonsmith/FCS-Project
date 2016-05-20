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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FCS_Funding.Views;
using FCS_DataTesting;
using System.Collections.ObjectModel;
using FCS_Funding.Models;
using System.Data;

namespace FCS_Funding.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Report_GrantDonations.xaml
    /// </summary>
    public partial class Report_GrantDonations : UserControl
    {
        FCS_DBModel db;

        public List<Grant> GrantList = new List<Grant>();
        public List<Session> GrantSessionList = new List<Session>();
        public ObservableCollection<GrantsDataGrid> Grants { get; set; }
        public ObservableCollection<SessionsGrid> Sessions { get; set; }
        public Report_GrantDonations()
        {
            InitializeComponent();
        }
        public class Grant
        {
            public String GrantName { get; set; }
            public String DonationAmount { get; set; }
            public String DonationAmountRemaining { get; set; }
            public String DonationDate { get; set; }
            public String DonationExpirationDate { get; set; }
            public String DonationID { get; set; }
            public String DonorID { get; set; }
            public String GrantProposalID { get; set; }
        };
        public class Session
        {
            public String ClientName { get; set; }
            public String Sessions { get; set; }
            public String Amount { get; set; }
            public String Date { get; set; }
            public String Balance { get; set; }

        };


        private void Grant_DataGridReport_Loaded(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel();
            var join = from d in db.Donations
                        join dr in db.Donors on d.DonorID equals dr.DonorID
                        join gp in db.GrantProposals on dr.DonorID equals gp.DonorID
                        where gp.GrantStatus == "Accepted" && d.GrantProposalID == gp.GrantProposalID
                        select new GrantsDataGrid
                        {
                            GrantName = gp.GrantName,
                            DonationAmount = d.DonationAmount,
                            DonationAmountRemaining = d.DonationAmountRemaining,
                            DonationDate = d.DonationDate,
                            ExpirationDate = d.DonationExpirationDate,
                            DonationID = d.DonationID,
                            DonorID = dr.DonorID,
                            GrantProposalID = gp.GrantProposalID
                        };

            //GrantsDataGrid g1 = new GrantsDataGrid("Cross Charitable Foundation", 1024.25M, DateTime.Now, DateTime.Now, "Charitable Minds", "We wanted to donate", 1024.25M);
            //Grants = new ObservableCollection<GrantsDataGrid>();
            //Grants.Add(g1);
            //var Test = join1.ToList();
            var grid = sender as DataGrid;
            grid.ItemsSource = join.ToList();
        }

        private void Grant_DataGridReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var db = new FCS_DBModel();
            DataGrid dg = sender as DataGrid;

            GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
            int GID = p.GrantProposalID;
            //MessageBox.Show("GID: " + GID);
            var join = from g in db.GrantProposals
                        join d in db.Donations on g.GrantProposalID equals d.GrantProposalID
                        join ex in db.Expenses on d.DonationID equals ex.DonationID
                        join pa in db.Patients on ex.PatientID equals pa.PatientID
                        join ap in db.Appointments on ex.AppointmentID equals ap.AppointmentID
                        where g.GrantProposalID == GID
                        select new SessionsDataGrid
                        {
                            ClientName = pa.PatientLastName + ", " + pa.PatientFirstName,
                            Sessions = 1,
                            Amount = ex.DonorBill,
                            Date = ap.AppointmentEndDate,
                            Balance = d.DonationAmountRemaining
                        };
            var grid = GrantSessions_DataGridReport as DataGrid;
            GrantSessions_DataGridReport.ItemsSource = join.ToList();
        }

    }
}
