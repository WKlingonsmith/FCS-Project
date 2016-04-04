namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Donation")]
    public partial class Donation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Donation()
        {
            DonationPurposes = new HashSet<DonationPurpose>();
            Expenses = new HashSet<Expense>();
            In_Kind_Item = new HashSet<In_Kind_Item>();
            In_Kind_Service = new HashSet<In_Kind_Service>();
        }

        public int DonationID { get; set; }

        public int? EventID { get; set; }

        public int DonorID { get; set; }

        public int? GrantProposalID { get; set; }

        public bool Restricted { get; set; }

        public bool InKind { get; set; }

        [Column(TypeName = "money")]
        public decimal DonationAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal DonationAmountRemaining { get; set; }

        [Column(TypeName = "date")]
        public DateTime DonationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DonationExpirationDate { get; set; }

        public virtual Donor Donor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonationPurpose> DonationPurposes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expense> Expenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<In_Kind_Item> In_Kind_Item { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<In_Kind_Service> In_Kind_Service { get; set; }
    }
}
