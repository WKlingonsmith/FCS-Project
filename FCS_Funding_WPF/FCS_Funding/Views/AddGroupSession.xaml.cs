using FCS_DataTesting;
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
    /// Interaction logic for AddGroupSession.xaml
    /// </summary>
    public partial class AddGroupSession : Window
    {
        public int ExpenseTypeID { get; set; }
        public int StaffID { get; set; }
        public DateTime ExpenseDueDate { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        PatientGrid Individual { get; set; }
        
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public int ClientOQNumber { get; set; }

        public decimal PatientBill { get; set; }
        public decimal DonorBill { get; set; }

        public AddGroupSession(int expenseTypeID, PatientGrid individual, int staffID, DateTime expenseDueDate, DateTime startDateTime, DateTime endDateTime)
        {
            ClientFirstName = individual.FirstName;
            ClientLastName = individual.LastName;
            ClientOQNumber = individual.PatientOQ;
            ExpenseTypeID = expenseTypeID;
            Individual = individual;
            StaffID = staffID;
            ExpenseDueDate = expenseDueDate;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            InitializeComponent();
        }

        private void Add_Expense(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            try
            {
                string grant = Grant.SelectedValue.ToString();
                var grantproposalID = (from g in db.GrantProposals
                                       where g.GrantName == grant
                                       select g.GrantProposalID).Distinct().FirstOrDefault();
                var donationID = (from d in db.Donations
                                  where d.GrantProposalID == grantproposalID
                                  select d.DonationID).Distinct().FirstOrDefault();
                var donation = (from d in db.Donations
                                where d.GrantProposalID == grantproposalID
                                select d).First();
                if (donation.DonationAmountRemaining >= DonorBill)
                {
                    try
                    {
                        Models.Appointment a = new Models.Appointment();
                        a.StaffID = StaffID;
                        a.AppointmentStartDate = StartDateTime;
                        a.AppointmentEndDate = EndDateTime;
                        db.Appointments.Add(a);
                        db.SaveChanges();

                        Models.Expense expense = new Models.Expense();

                        expense.ExpenseTypeID = ExpenseTypeID;
                        expense.DonationID = donationID;
                        expense.PatientID = Individual.PatientID;
                        expense.AppointmentID = a.AppointmentID;
                        expense.ExpenseDueDate = ExpenseDueDate;
                        expense.DonorBill = DonorBill;
                        expense.PatientBill = PatientBill;
                        expense.TotalExpenseAmount = DonorBill + PatientBill;
                        if (ExpensePaidDate.IsEnabled == true) { expense.ExpensePaidDate = Convert.ToDateTime(ExpensePaidDate.ToString()); }
                        db.Expenses.Add(expense);
                        db.SaveChanges();

                        donation.DonationAmountRemaining = donation.DonationAmountRemaining - DonorBill;
                        db.SaveChanges();

                        MessageBox.Show("Successfully added In_Kind Service");
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Please make sure all fields are correct");
                    }
                }
                else
                {
                    MessageBox.Show("That grant does not have enough money.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Make sure to select a grant.");
            }
        }

        private void Grants_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            var query = (from o in db.GrantProposals
                         where o.GrantStatus == "Accepted"
                         select o.GrantName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }
        private void Change_Paid_Bill(object sender, RoutedEventArgs e)
        {
            if (IsPaid.IsChecked.Value)
            {
                ExpensePaidDate.IsEnabled = true;
            }
            else
            {
                ExpensePaidDate.IsEnabled = false;
            }
        }
    }
}
