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
    /// Interaction logic for Report_Demographics.xaml
    /// </summary>
    public partial class Report_Demographics : UserControl
    {
        FCS_DBModel db;

        public Report_Demographics()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var db = new FCS_DBModel();
            int DemographicsReportDayStart, DemographicsReportDayEnd, DemographicsReportMonthStart, DemographicsReportMonthEnd, DemographicsReportYear;
            if (DemographicsReportMonth_comboBox.Text == "All")
            {
                DemographicsReportDayStart = 1;
                DemographicsReportDayEnd = DateTime.DaysInMonth(int.Parse(DemographicsReportYear_comboBox.Text), 12);
                DemographicsReportMonthStart = 1;
                DemographicsReportMonthEnd = 12;
                DemographicsReportYear = Int32.Parse(DemographicsReportYear_comboBox.Text);
            }
            else
            {
                DemographicsReportDayStart = 1;
                DemographicsReportDayEnd = DateTime.DaysInMonth(Int32.Parse(DemographicsReportYear_comboBox.Text), DemographicsReportMonth_comboBox.SelectedIndex);
                DemographicsReportMonthStart = DemographicsReportMonth_comboBox.SelectedIndex;
                DemographicsReportMonthEnd = DemographicsReportMonth_comboBox.SelectedIndex;
                DemographicsReportYear = Int32.Parse(DemographicsReportYear_comboBox.Text);
            }
            DateTime requestedDateStart = Convert.ToDateTime(DemographicsReportMonthStart + "/" + DemographicsReportDayStart + "/" + DemographicsReportYear);
            DateTime requestedDateEnd = Convert.ToDateTime(DemographicsReportMonthEnd + "/" + DemographicsReportDayEnd + "/" + DemographicsReportYear);
            MessageBox.Show("Start: " + requestedDateStart + " End: " + requestedDateEnd);
            db = new FCS_DBModel();

            // Get Patients
            var join = from gp in db.GrantProposals
                       select new { GrantNameOut = gp.GrantName, GrantProposalIDOut = gp.GrantProposalID };
            foreach (var q in join)
            {
                Console.WriteLine("Name:" + q.GrantNameOut + "GrantProposalID:" + q.GrantProposalIDOut);
            }

            var newPatients = (from p in db.Patients where p.NewClientIntakeHour > requestedDateStart && p.NewClientIntakeHour > requestedDateEnd select p.PatientID).Count();
            var totalPatients = (from p in db.Patients select p.PatientID).Count();
            var ongoingPatients = totalPatients - newPatients;
            //var Patients = db.Patients.Count();
            //where g.GrantProposalID == GID && ap.AppointmentEndDate > requestedDateStart && ap.AppointmentEndDate < requestedDateEnd
            Console.WriteLine("Patients - New:" + newPatients + " Ongoing:" + ongoingPatients + " Total:" + totalPatients);
        }

        private void Report_Demographics_Loaded(object sender, RoutedEventArgs e)
        {
            DemographicsReportMonth_comboBox.SelectedIndex = 0;//.Text = (DateTime.Now.Year.ToString());
            DemographicsReportYear_comboBox.SelectedIndex = 0;
        }
    }
}
