using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Donor
    {
        public Donor()
        {
            this.Donations = new List<Donation>();
            this.DonorContacts = new List<DonorContact>();
            this.GrantProposals = new List<GrantProposal>();
            this.Reminders = new List<Reminder>();
        }
        public Donor(string fn, string ln, string dt, string on, string a1, string a2, string s, string c, string z)
        {
            this.DonorFirstName = fn;
            this.DonorLastName = ln;
            this.DonorType = dt;
            this.OrganizationName = on;
            this.DonorAddress1 = a1;
            this.DonorAddress2 = a2;
            this.DonorState = s;
            this.DonorCity = c;
            this.DonorZip = z;
            this.Donations = new List<Donation>();
            this.DonorContacts = new List<DonorContact>();
            this.GrantProposals = new List<GrantProposal>();
            this.Reminders = new List<Reminder>();
        }

        public int DonorID { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorType { get; set; }
        public string OrganizationName { get; set; }
        public string DonorAddress1 { get; set; }
        public string DonorAddress2 { get; set; }
        public string DonorState { get; set; }
        public string DonorCity { get; set; }
        public string DonorZip { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<DonorContact> DonorContacts { get; set; }
        public virtual ICollection<GrantProposal> GrantProposals { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}
