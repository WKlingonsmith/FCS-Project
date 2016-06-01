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
using System.Data.Objects.SqlClient;
using System.Data.Entity;
namespace FCS_Funding.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Report_Demographics.xaml
    /// </summary>
    public partial class Report_Demographics : UserControl
    {
        //FCS_DBModel db;

        public Report_Demographics()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*int DemographicsReportDayStart, DemographicsReportDayEnd, DemographicsReportMonthStart, DemographicsReportMonthEnd, DemographicsReportYear;
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
            */
            //MessageBox.Show("Start: " + requestedDateStart + " End: " + requestedDateEnd);
            if (demographicsReportFrom_datepicker.SelectedDate != null && demographicsReportFrom_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue && demographicsReportTo_datepicker.SelectedDate != null && demographicsReportTo_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue)
            {
                //Console.WriteLine("Trying to query database");
                var db = new FCS_DBModel();
                DateTime requestedDateStart = DateTime.Parse(demographicsReportFrom_datepicker.Text);
                DateTime requestedDateEnd = DateTime.Parse(demographicsReportTo_datepicker.Text);
                var listOfAllSessions = from ap in db.Appointments
                           join st in db.Staff on ap.StaffID equals st.StaffID
                           join ex in db.Expenses on ap.AppointmentID equals ex.AppointmentID
                           join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
                           join pa in db.Patients on ex.PatientID equals pa.PatientID
                           join ph in db.PatientHouseholds on pa.HouseholdID equals ph.HouseholdID
                           where ap.AppointmentStartDate > requestedDateStart && ap.AppointmentEndDate < requestedDateEnd
                           select new
                           {
                               patientID = pa.PatientID,
                               patientFirstName = pa.PatientFirstName,
                               patientLastName = pa.PatientLastName,
                               staff = st.StaffLastName + ", " + st.StaffFirstName,
                               patientIntakeTimeDate = pa.NewClientIntakeHour,
                               appointmentDateTimeStart = ap.AppointmentStartDate,
                               appointmentDateTimeEnd = ap.AppointmentEndDate,
                               isHeadOfHousehold = pa.IsHead,
                               householdID = pa.HouseholdID,
                               householdCount = ph.HouseholdPopulation,
                               ageGroup = pa.PatientAgeGroup,
                               gender = pa.PatientGender,
                               ethnicity = pa.PatientEthnicity,
                               patientIncome = ph.HouseholdIncomeBracket,
                               patientCounty = ph.HouseholdCounty,
                               fundingSource = ex.DonationID,
                               expenseTypeID = ex.ExpenseTypeID,
                               typeofCx = ap.AppointmentCancelationType,
                               patientProblems = (from pp in db.PatientProblems
                                                  where pp.PatientID == pa.PatientID
                                                  select new
                                                  {
                                                      problemID = pp.PatientProblemID
                                                  })
                           };

                int newPatients = 0;
                int ongoingPatients = 0;
                double totalMinutesofService = 0;

                foreach (var q in listOfAllSessions)
                {
                    DateTime appointmentStartTime = (DateTime)q.appointmentDateTimeStart;
                    DateTime appointmentEndTime = (DateTime)q.appointmentDateTimeEnd;
                    if ((q.patientIntakeTimeDate > requestedDateStart) && (q.patientIntakeTimeDate < requestedDateEnd))
                    {
                        newPatients++;
                    } else
                    {
                        ongoingPatients++;
                    }
                    totalMinutesofService += appointmentEndTime.Subtract(appointmentStartTime).TotalMinutes;
                }
                double totalHoursofService =totalMinutesofService / 60;

                int sessionTypeIndiv = listOfAllSessions.Where(x => x.expenseTypeID.Equals(1)).Count();
                int sessionTypeGroup = listOfAllSessions.Where(x => x.expenseTypeID.Equals(2)).Count();
                int sessionTypeFamily = listOfAllSessions.Where(x => x.expenseTypeID.Equals(3)).Count();

                int HoHM = listOfAllSessions.Where(x => (x.isHeadOfHousehold.Equals(true)) && x.gender.Equals("Male")).Count();
                int HoHF = listOfAllSessions.Where(x => (x.isHeadOfHousehold.Equals(true)) && x.gender.Equals("Female")).Count();

                int ageBracket1 = listOfAllSessions.Where(x => x.ageGroup.Contains("0-5")).GroupBy(x=>x.patientID).Count();
                int ageBracket2 = listOfAllSessions.Where(x => x.ageGroup.Contains("6-11")).Count();
                int ageBracket3 = listOfAllSessions.Where(x => x.ageGroup.Contains("12-17")).Count();
                int ageBracket4 = listOfAllSessions.Where(x => x.ageGroup.Contains("18-23")).Count();
                int ageBracket5 = listOfAllSessions.Where(x => x.ageGroup.Contains("24-44")).Count();
                int ageBracket6 = listOfAllSessions.Where(x => x.ageGroup.Contains("45-54")).Count();
                int ageBracket7 = listOfAllSessions.Where(x => x.ageGroup.Contains("55-69")).Count();
                int ageBracket8 = listOfAllSessions.Where(x => x.ageGroup.Contains("70+")).Count();

                int genderMale = listOfAllSessions.Where(x => x.gender.Equals("Male")).Count();
                int genderFemale = listOfAllSessions.Where(x => x.gender.Equals("Female")).Count();

                int ethnicity1 = listOfAllSessions.Where(x => x.ethnicity.Equals("African American")).Count();
                int ethnicity2 = listOfAllSessions.Where(x => x.ethnicity.Equals("Native/Alaskan")).Count();
                int ethnicity3 = listOfAllSessions.Where(x => x.ethnicity.Equals("Pacific Islander")).Count();
                int ethnicity4 = listOfAllSessions.Where(x => x.ethnicity.Equals("Asian")).Count();
                int ethnicity5 = listOfAllSessions.Where(x => x.ethnicity.Equals("Caucasian")).Count();
                int ethnicity6 = listOfAllSessions.Where(x => x.ethnicity.Equals("Hispanic")).Count();
                int ethnicity7 = listOfAllSessions.Where(x => x.ethnicity.Equals("Other")).Count();

                int incomeBracket1 = listOfAllSessions.Where(x => x.patientIncome.Equals("$0-9,999")).Count();
                int incomeBracket2 = listOfAllSessions.Where(x => x.patientIncome.Equals("$10,000-14,999")).Count();
                int incomeBracket3 = listOfAllSessions.Where(x => x.patientIncome.Equals("$15,000-24,999")).Count();
                int incomeBracket4 = listOfAllSessions.Where(x => x.patientIncome.Equals("$25,000-34,999")).Count();
                int incomeBracket5 = listOfAllSessions.Where(x => x.patientIncome.Equals("$35,000+")).Count();

                int county1 = listOfAllSessions.Where(x => x.patientCounty.Equals("Weber")).Count();
                int county2 = listOfAllSessions.Where(x => x.patientCounty.Equals("Davis")).Count();
                int county3 = listOfAllSessions.Where(x => x.patientCounty.Equals("DCLC")).Count();
                int county4 = listOfAllSessions.Where(x => x.patientCounty.Equals("Morgan")).Count();
                int county5 = listOfAllSessions.Where(x => x.patientCounty.Equals("Box Elder")).Count();
                int county6 = listOfAllSessions.Where(x => x.patientCounty.Equals("Other")).Count();
            } else
            {
                MessageBox.Show("Please pick a valid starting and ending date prior to clicking Generate Report");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            demographicsReportFrom_datepicker.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            demographicsReportTo_datepicker.SelectedDate = new DateTime(DateTime.Now.Year, 12, 31);
            var db = new FCS_DBModel();
            var fundingSourceList = (from don in db.Donations.AsEnumerable()
                                     join donor in db.Donors on don.DonationID equals donor.DonorID
                                     where don.GrantProposalID == null
                                     select new
                                     {
                                         fundingSourceID = don.DonationID,
                                         fundingSource = donor.OrganizationName
                                     }).ToList();

            var grantSourceList = (from don in db.Donations.AsEnumerable()
                                   join gp in db.GrantProposals on don.GrantProposalID equals gp.GrantProposalID
                                   where don.GrantProposalID != null
                                   select new
                                   {
                                       fundingSourceID = don.DonationID,
                                       fundingSource = gp.GrantName
                                   }).ToList();

            fundingSourceList.AddRange(grantSourceList);
            reportDemoFundingSource_listbox.ItemsSource = fundingSourceList;

            db = new FCS_DBModel();
            var problemList = (from pp in db.Problems
                               orderby pp.ProblemType
                               select new
                               {
                                   problemType = pp.ProblemType
                               }).Distinct();
            reportDemoProblems_listbox.ItemsSource = problemList.ToList();

            reportDemoFundingSource_listbox.SelectAll();
            reportDemoProblems_listbox.SelectAll();
        }
    }
}
