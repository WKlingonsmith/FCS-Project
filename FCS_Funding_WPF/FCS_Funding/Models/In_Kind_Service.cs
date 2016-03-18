using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class In_Kind_Service
    {
        public In_Kind_Service()
        { }
        public In_Kind_Service(int donationID, DateTime startDate, DateTime endDate,
            decimal rate, string description, double hours, decimal value)
        {
            DonationID = donationID;
            StartDateTime = startDate;
            EndDateTime = endDate;
            RatePerHour = rate;
            ServiceDescription = description;
            ServiceLength = hours;
            ServiceValue = value;
        }
        public int ServiceID { get; set; }
        public int DonationID { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public decimal RatePerHour { get; set; }
        public string ServiceDescription { get; set; }
        public double ServiceLength { get; set; }
        public decimal ServiceValue { get; set; }
        public virtual Donation Donation { get; set; }
    }
}
