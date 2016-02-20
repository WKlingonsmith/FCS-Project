using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Reminder
    {
        public int ReminderID { get; set; }
        public int DonorID { get; set; }
        public System.DateTime ReminderDate { get; set; }
        public string ReminderDescription { get; set; }
        public virtual Donor Donor { get; set; }
    }
}
