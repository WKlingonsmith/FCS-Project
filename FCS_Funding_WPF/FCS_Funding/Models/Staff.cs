using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Staff
    {
        public Staff()
        {
            this.Appointments = new List<Appointment>();
        }

        public int StaffID { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffTitle { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
