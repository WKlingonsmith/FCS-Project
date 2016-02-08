namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonorContact")]
    public partial class DonorContact
    {
        [Key]
        public int ContactID { get; set; }

        public int DonorID { get; set; }

        [Required]
        [StringLength(30)]
        public string ContactFirstName { get; set; }

        [StringLength(30)]
        public string ContactLastName { get; set; }

        [StringLength(10)]
        public string ContactPhone { get; set; }

        [StringLength(700)]
        public string ContactEmail { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
