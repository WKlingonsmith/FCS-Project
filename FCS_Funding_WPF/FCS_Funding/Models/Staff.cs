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
        public Staff(string firstName, string lastName, string title, string userName, string password, string role)
        {
            StaffFirstName = firstName;
            StaffLastName = lastName;
            StaffTitle = title;
            StaffUserName = userName;
            StaffPassword = password;
            StaffDBRole = role;
            this.Appointments = new List<Appointment>();
        }

        public int StaffID { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffTitle { get; set; }
        public string StaffUserName { get; set; }
        public string StaffPassword { get; set; }
        public string StaffDBRole { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
