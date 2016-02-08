namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purpose")]
    public partial class Purpose
    {
        public Purpose()
        {
            DonationPurpose = new HashSet<DonationPurpose>();
        }

        public int PurposeID { get; set; }

        [Required]
        [StringLength(100)]
        public string PurposeName { get; set; }

        [Required]
        [StringLength(5000)]
        public string PurposeDescription { get; set; }

        public virtual ICollection<DonationPurpose> DonationPurpose { get; set; }
    }
}
