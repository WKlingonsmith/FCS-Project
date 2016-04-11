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
    /// Interaction logic for AddSession.xaml
    /// </summary>
    public partial class AddSession : Window
    {
        public string BeginHour { get; set; }
        public string BeginMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public int PatientID { get; set; }
        public decimal PatientBill { get; set; }
        public decimal DonorBill { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseDescription { get; set; }


        public AddSession(int patientID)
        {
            PatientID = patientID;
            InitializeComponent();
        }

        private void Add_Appointment(object sender, RoutedEventArgs e)
        {
            if (AMPM_Start.SelectedValue.ToString() == "PM" && Convert.ToInt32(BeginHour) != 12)
            {
                BeginHour = (Convert.ToInt32(BeginHour) + 12).ToString();
            }
            if (AMPM_End.SelectedValue.ToString() == "PM" && Convert.ToInt32(EndHour) != 12)
            {
                EndHour = (Convert.ToInt32(EndHour) + 12).ToString();
            }
            if (AMPM_Start.SelectedValue.ToString() == "AM" && Convert.ToInt32(BeginHour) == 12)
            {
                BeginHour = (Convert.ToInt32(BeginHour) - 12).ToString();
            }
            if (AMPM_End.SelectedValue.ToString() == "AM" && Convert.ToInt32(EndHour) == 12)
            {
                EndHour = (Convert.ToInt32(EndHour) - 12).ToString();
            }
            DateTime expenseDueDate = Convert.ToDateTime(ExpenseDueDate.ToString());
            DateTime help = Convert.ToDateTime(DateRecieved.ToString());
            DateTime startDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(BeginHour), Convert.ToInt32(BeginMinute), 0);
            DateTime endDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(EndHour), Convert.ToInt32(EndMinute), 0);
            string[] separators = new string[] { ", " };
            string staff = Staff.SelectedValue.ToString();
            string grant = Grant.SelectedValue.ToString();            
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            MessageBox.Show(PatientBill + "\n" + DonorBill + "\n" + startDateTime + "\n" + endDateTime + "\n" + expenseDueDate + "\n" + staff, grant);
            string[] words = staff.Split(separators, StringSplitOptions.None);
            string FName = words[0]; string LName = words[1]; string username = words[2];
            var staffID = (from dc in db.Staffs
                            where dc.StaffFirstName == FName && dc.StaffLastName == LName && dc.StaffUserName == username
                           select dc.StaffID).Distinct().FirstOrDefault();
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

                Models.ExpenseType extype = new Models.ExpenseType();
                extype.ExpenseType1 = ExpenseType;
                extype.ExpenseDescription = ExpenseDescription;
                db.ExpenseTypes.Add(extype);
                db.SaveChanges();

                Models.Appointment a = new Models.Appointment();
                a.StaffID = staffID;
                a.AppointmentStartDate = startDateTime;
                a.AppointmentEndDate = endDateTime;
                db.Appointments.Add(a);
                db.SaveChanges();

                Models.Expense expense = new Models.Expense();

                expense.ExpenseTypeID = extype.ExpenseTypeID;
                expense.DonationID = donationID;
                expense.PatientID = PatientID;
                expense.AppointmentID = a.AppointmentID;
                expense.ExpenseDueDate = expenseDueDate;
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
            else
            {
                MessageBox.Show("That grant does not have enough money.");
            }
        }
        private void AM_PM_Dropdown(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>() { "AM", "PM" };
        }

        private void Hour_LostFocus(object sender, RoutedEventArgs e)
        {

            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (value > 12)
                        textbox.Text = "12";
                    else if (value < 1)
                        textbox.Text = "1";
                }
                catch (Exception ex)
                {
                    textbox.Text = "";
                    MessageBox.Show("You inserted a character");
                }
            }

        }

        private void Minute_LostFocus(object sender, RoutedEventArgs e)
        {
            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (textbox.Text.Length == 1)
                    {
                        textbox.Text = "0" + textbox.Text;
                    }
                    else if (value > 59)
                    {
                        textbox.Text = "59";
                    }
                    else if (value < 0)
                    {
                        textbox.Text = "00";
                    }
                }
                catch (Exception ex)
                {
                    textbox.Text = "";
                    MessageBox.Show("You inserted a character");
                }
            }
        }

        private void Hour_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.Text = "12";
        }

        private void Minute_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.Text = "00";
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

        private void Staff_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            var query = (from o in db.Staffs
                         select o.StaffFirstName + ", " + o.StaffLastName + ", " + o.StaffUserName).ToList();

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
