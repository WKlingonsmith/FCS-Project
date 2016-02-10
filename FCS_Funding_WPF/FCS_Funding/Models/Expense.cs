namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Expense")]
    public partial class Expense
    {
        //Ken made this constructor
        Expense(int eID, int etID, int dID, int pID, int aID, DateTime eDueDate, DateTime ePaidDate, decimal eAmount)
        {
            ExpenseID = eID;
            ExpenseTypeID = etID;
            DonationID = dID;
            PatientID = pID;
            AppointmentID = aID;
            ExpenseDueDate = eDueDate;
            ExpensePaidDate = ePaidDate;
            ExpenseAmount = eAmount;
        }

        public int ExpenseID { get; set; }

        public int ExpenseTypeID { get; set; }

        public int? DonationID { get; set; }

        public int? PatientID { get; set; }

        public int? AppointmentID { get; set; }

        [Column(TypeName = "date")]
        public DateTime ExpenseDueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpensePaidDate { get; set; }

        [Column(TypeName = "money")]
        public decimal ExpenseAmount { get; set; }

        public virtual Appointment Appointment { get; set; }

        public virtual Donation Donation { get; set; }

        public virtual ExpenseType ExpenseType { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
