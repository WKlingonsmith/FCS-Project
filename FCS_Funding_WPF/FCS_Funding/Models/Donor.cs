namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Donor")]
    public partial class Donor
    {
        public Donor()
        {
            Donation = new HashSet<Donation>();
            DonorContact = new HashSet<DonorContact>();
            GrantProposal = new HashSet<GrantProposal>();
            Reminder = new HashSet<Reminder>();
        }

        public int DonorID { get; set; }

        [StringLength(50)]
        public string DonorFirstName { get; set; }

        [StringLength(50)]
        public string DonorLastName { get; set; }

        [Required]
        [StringLength(50)]
        public string DonorType { get; set; }

        [StringLength(250)]
        public string OrganizationName { get; set; }

        [StringLength(50)]
        public string DonorAddress1 { get; set; }

        [StringLength(50)]
        public string DonorAddress2 { get; set; }

        [StringLength(2)]
        public string DonorState { get; set; }

        [StringLength(200)]
        public string DonorCity { get; set; }

        [StringLength(10)]
        public string DonorZip { get; set; }

        public virtual ICollection<Donation> Donation { get; set; }

        public virtual ICollection<DonorContact> DonorContact { get; set; }

        public virtual ICollection<GrantProposal> GrantProposal { get; set; }

        public virtual ICollection<Reminder> Reminder { get; set; }
    }
}
