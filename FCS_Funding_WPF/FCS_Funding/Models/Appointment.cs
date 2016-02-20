using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            this.Expenses = new List<Expense>();
        }

        public int AppointmentID { get; set; }
        public int StaffID { get; set; }
        public System.DateTime AppointmentStartDate { get; set; }
        public System.DateTime AppointmentEndDate { get; set; }
        public string AppointmentCancelationType { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
