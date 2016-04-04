namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class In_Kind_Service
    {
        [Key]
        public int ServiceID { get; set; }

        public int DonationID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        [Column(TypeName = "money")]
        public decimal RatePerHour { get; set; }

        [Required]
        [StringLength(5000)]
        public string ServiceDescription { get; set; }

        public double ServiceLength { get; set; }

        [Column(TypeName = "money")]
        public decimal ServiceValue { get; set; }

        public virtual Donation Donation { get; set; }
    }
}
