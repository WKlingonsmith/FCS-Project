namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class In_Kind_Item
    {
        [Key]
        public int ItemID { get; set; }

        public int DonationID { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

        [Required]
        [StringLength(5000)]
        public string ItemDescription { get; set; }

        public virtual Donation Donation { get; set; }
    }
}
