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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Donor()
        {
            Donations = new HashSet<Donation>();
            DonorContacts = new HashSet<DonorContact>();
            GrantProposals = new HashSet<GrantProposal>();
            Reminders = new HashSet<Reminder>();
        }

        public int DonorID { get; set; }

        [Required]
        [StringLength(50)]
        public string DonorType { get; set; }

        [StringLength(250)]
        public string OrganizationName { get; set; }

        [StringLength(50)]
        public string DonorAddress1 { get; set; }

        [StringLength(50)]
        public string DonorAddress2 { get; set; }

        [StringLength(20)]
        public string DonorState { get; set; }

        [StringLength(200)]
        public string DonorCity { get; set; }

        [StringLength(20)]
        public string DonorZip { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donation> Donations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonorContact> DonorContacts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrantProposal> GrantProposals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}
