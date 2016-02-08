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
        public Donation()
        {
            DonationPurpose = new HashSet<DonationPurpose>();
            Expense = new HashSet<Expense>();
            In_Kind_Item = new HashSet<In_Kind_Item>();
            In_Kind_Service = new HashSet<In_Kind_Service>();
        }

        public int DonationID { get; set; }

        public int? EventID { get; set; }

        public int DonorID { get; set; }

        public int? RequestForPersonalID { get; set; }

        public bool Restricted { get; set; }

        public bool InKind { get; set; }

        [Column(TypeName = "money")]
        public decimal DonationAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime DonationDate { get; set; }

        public virtual Donor Donor { get; set; }

        public virtual ICollection<DonationPurpose> DonationPurpose { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }

        public virtual ICollection<In_Kind_Item> In_Kind_Item { get; set; }

        public virtual ICollection<In_Kind_Service> In_Kind_Service { get; set; }
    }
}
