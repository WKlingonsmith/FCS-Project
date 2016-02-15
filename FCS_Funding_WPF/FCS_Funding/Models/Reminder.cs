namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reminder")]
    public partial class Reminder
    {
        //constructor created by Ken
        public Reminder(int remindID, int donorID, DateTime remindDate, string description)
        {
                ReminderID = remindID;
                DonorID = donorID;
                ReminderDate = remindDate;
                ReminderDescription = description;
        }

        public int ReminderID { get; set; }

        public int DonorID { get; set; }

        public DateTime ReminderDate { get; set; }

        [StringLength(5000)]
        public string ReminderDescription { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
