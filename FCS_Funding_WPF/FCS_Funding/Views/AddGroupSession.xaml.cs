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
        public string ClientOQNumber { get; set; }

        public decimal PatientBill { get; set; }
        public decimal DonorBill { get; set; }
        public int AppointmentID { get; set; }

        public AddGroupSession(int expenseTypeID, PatientGrid individual, int staffID, DateTime expenseDueDate, DateTime startDateTime, DateTime endDateTime, int appointmentID)
        {
            ClientFirstName = individual.FirstName;
            ClientLastName = individual.LastName;
            ClientOQNumber = individual.PatientOQ;
            ExpenseTypeID = expenseTypeID;
            Individual = individual;
            StaffID = staffID;
            AppointmentID = appointmentID;
            ExpenseDueDate = expenseDueDate;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            InitializeComponent();
        }

        private void Add_Expense(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            try
            {
                //Taking the money from a grant
                if (DonorDeduction.IsChecked.Value)
                {
                    string grant = Grant.SelectedValue.ToString();
                    var grantproposalID = (from g in db.GrantProposals
                                           where g.GrantName == grant
                                           select g.GrantProposalID).Distinct().FirstOrDefault();
                    var grantDonation = (from d in db.Donations
                                         where d.GrantProposalID == grantproposalID
                                         select d).First();
                    if (grantDonation.DonationAmountRemaining >= DonorBill)
                    {
                        Models.Expense expense = new Models.Expense();

                        var donationID = (from d in db.Donations
                                          where d.GrantProposalID == grantproposalID
                                          select d.DonationID).Distinct().FirstOrDefault();
                        expense.DonationID = donationID;

                        expense.ExpenseTypeID = ExpenseTypeID;
                        expense.PatientID = Individual.PatientID;
                        expense.AppointmentID = AppointmentID;
                        expense.ExpenseDueDate = ExpenseDueDate;
                        expense.DonorBill = DonorBill;
                        expense.PatientBill = PatientBill;
                        expense.TotalExpenseAmount = DonorBill + PatientBill;
                        if (ExpensePaidDate.IsEnabled == true) { expense.ExpensePaidDate = Convert.ToDateTime(ExpensePaidDate.ToString()); }

                        db.Expenses.Add(expense);
                        db.SaveChanges();

                        grantDonation.DonationAmountRemaining = grantDonation.DonationAmountRemaining - DonorBill;
                        db.SaveChanges();

                        MessageBox.Show("Successfully added Expense");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("That grant does not have enough money.");
                    }
                }
                //taking the money from a money donation.
                else
                {
                    int donationID = Convert.ToInt32(MoneyDonation.SelectedValue.ToString());

                    var donation = (from d in db.Donations
                                    where d.DonationID == donationID
                                    select d).First();
                    if (donation.DonationAmountRemaining >= DonorBill)
                    {
                        Models.Expense expense = new Models.Expense();

                        expense.ExpenseTypeID = ExpenseTypeID;
                        expense.DonationID = donationID;
                        expense.PatientID = Individual.PatientID;
                        expense.AppointmentID = AppointmentID;
                        expense.ExpenseDueDate = ExpenseDueDate;
                        expense.DonorBill = DonorBill;
                        expense.PatientBill = PatientBill;
                        expense.TotalExpenseAmount = DonorBill + PatientBill;
                        if (ExpensePaidDate.IsEnabled == true) { expense.ExpensePaidDate = Convert.ToDateTime(ExpensePaidDate.ToString()); }

                        db.Expenses.Add(expense);
                        db.SaveChanges();
                        donation.DonationAmountRemaining = donation.DonationAmountRemaining - DonorBill;
                        db.SaveChanges();

                        MessageBox.Show("Successfully added Expense");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("That donation does not have enough money.");
                    }
                }
            }
            catch
            {
                MessageBox.Show(DonorBill.ToString());
                if (DonorBill == 0)
                {
                    try
                    {
                        Models.Expense expense = new Models.Expense();

                        expense.ExpenseTypeID = ExpenseTypeID;
                        expense.PatientID = Individual.PatientID;
                        expense.AppointmentID = AppointmentID;
                        expense.ExpenseDueDate = ExpenseDueDate;
                        expense.DonorBill = DonorBill;
                        expense.PatientBill = PatientBill;
                        expense.TotalExpenseAmount = DonorBill + PatientBill;
                        if (ExpensePaidDate.IsEnabled == true) { expense.ExpensePaidDate = Convert.ToDateTime(ExpensePaidDate.ToString()); }

                        db.Expenses.Add(expense);
                        db.SaveChanges();
                        MessageBox.Show("Successfully added Expense");
                        this.Close();
                    }
                    catch(Exception exc)
                    {
                        MessageBox.Show("Please make sure all fields are correct");
                    }
                }
                else
                {
                    MessageBox.Show("Make sure you select a grant");
                }
            }
        }

        private void Grants_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
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

        private void MoneyDonation_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var query = (from d in db.Donations
                         where d.GrantProposalID == null
                         select d.DonationID).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Change_Client_Bill(object sender, RoutedEventArgs e)
        {
            if (DonorDeduction.IsChecked.Value)
            {
                MoneyDonation.IsEnabled = false;
                Grant.IsEnabled = true;
            }
            else
            {
                MoneyDonation.IsEnabled = true;
                Grant.IsEnabled = false;
            }
        }
    }
}
