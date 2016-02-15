namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class In_Kind_Service
    {
        //constructor made by Ken
        public In_Kind_Service(int sID, int dID, DateTime start, DateTime end, decimal rate, string description)
        {
            ServiceID = sID;
            DonationID = dID;
            StartDateTime = start;
            EndDateTime = end;
            RatePerHour = rate;
            ServiceDescription = description;
        }

        [Key]
        public int ServiceID { get; set; }

        public int DonationID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        [Column(TypeName = "money")]
        public decimal RatePerHour { get; set; }

        [Required]
        [StringLength(5000)]
        public string ServiceDescription { get; set; }

        public virtual Donation Donation { get; set; }
    }
}
