using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Expense
    {
        public int ExpenseID { get; set; }
        public int ExpenseTypeID { get; set; }
        public Nullable<int> DonationID { get; set; }
        public Nullable<int> PatientID { get; set; }
        public Nullable<int> AppointmentID { get; set; }
        public System.DateTime ExpenseDueDate { get; set; }
        public Nullable<System.DateTime> ExpensePaidDate { get; set; }
        public decimal ExpenseAmount { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual Donation Donation { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
