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
    /// Interaction logic for UpdateSession.xaml
    /// </summary>
    public partial class UpdateSession : Window
    { 
        public SessionsGrid Session { get; set; }

        public UpdateSession(SessionsGrid sg)
        {
            Session = sg;
            InitializeComponent();
        }

        private void Delete_Expense(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Session?",
               "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();

                var expense = (from exp in db.Expenses
                               where exp.ExpenseID == Session.ExpenseID
                               select exp).First();

                var expenses = (from exp in db.Expenses
                               where exp.AppointmentID == expense.AppointmentID
                               select exp).ToList();
                int? DonationID = expense.DonationID;
                int? AppointmentID = expense.AppointmentID;

                db.Expenses.Remove(expense);

                //Add money back to the donation
                if (expense.DonorBill > 0)
                {
                    var donation = (from d in db.Donations
                                    where d.DonationID == DonationID
                                    select d).First();
                    donation.DonationAmountRemaining = donation.DonationAmountRemaining + expense.DonorBill;
                    db.SaveChanges(); 
                }
                if(expenses.Count == 1)
                {
                    var appt = (from d in db.Appointments
                                    where d.AppointmentID == AppointmentID
                                    select d).First();
                    db.Appointments.Remove(appt);
                    db.SaveChanges();
                }
                db.SaveChanges();

                MessageBox.Show("This session has been deleted.");
                this.Close();
            }
        }
    }
}
