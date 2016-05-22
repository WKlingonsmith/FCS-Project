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
            public int Sessions { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public decimal Balance { get; set; }

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
            decimal remainingBalance = p.DonationAmount;
            var join = from g in db.GrantProposals
                       join d in db.Donations on g.GrantProposalID equals d.GrantProposalID
                       join ex in db.Expenses on d.DonationID equals ex.DonationID
                       join pa in db.Patients on ex.PatientID equals pa.PatientID
                       join ap in db.Appointments on ex.AppointmentID equals ap.AppointmentID
                       where g.GrantProposalID == GID orderby ap.AppointmentEndDate
                       select new SessionsDataGrid
                       {
                           ClientName = pa.PatientLastName + ", " + pa.PatientFirstName,
                           Sessions = 1,
                           Amount = ex.DonorBill,
                           Date = ap.AppointmentEndDate,
                           Balance = 0  //All balance calculations are done after collection is generated
                       };

            //Clear list of previous sessions
            GrantSessionList.Clear();

            //Loop over join collection generated above to calculate the correct remaining balance for each row
            foreach (var joint in join)
            {
                remainingBalance = remainingBalance - joint.Amount;
                GrantSessionList.Add(new Session()
                {
                    ClientName = joint.ClientName,
                    Sessions = joint.Sessions,
                    Amount = joint.Amount,
                    Date = joint.Date,
                    Balance = remainingBalance
                });
            }
            // Clear datagrid or values will not update correctly
            GrantSessions_DataGridReport.ItemsSource = null;
            GrantSessions_DataGridReport.ItemsSource = GrantSessionList;
        }

        private void printGrantSessions_button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog PrintDlg = new System.Windows.Controls.PrintDialog();
            if ((bool)PrintDlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(PrintDlg.PrintableAreaWidth, PrintDlg.PrintableAreaHeight);
                GrantSessions_DataGridReport.Measure(pageSize);
                GrantSessions_DataGridReport.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                PrintDlg.PrintVisual(GrantSessions_DataGridReport, "Grant Sessions");

            }
        }
    }
}
