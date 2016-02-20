using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class In_Kind_Service
    {
        public int ServiceID { get; set; }
        public int DonationID { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public decimal RatePerHour { get; set; }
        public string ServiceDescription { get; set; }
        public virtual Donation Donation { get; set; }
    }
}
