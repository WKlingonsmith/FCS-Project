namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        public Appointment()
        {
            Expense = new HashSet<Expense>();
        }

        public int AppointmentID { get; set; }

        public int StaffID { get; set; }

        public DateTime AppointmentStartDate { get; set; }

        public DateTime AppointmentEndDate { get; set; }

        [StringLength(12)]
        public string AppointmentCancelationType { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
