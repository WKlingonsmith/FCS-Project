namespace FCS_Funding.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class View_GrantProposals
    {
        [StringLength(250)]
        public string OrganizationName { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string GrantName { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime SubmissionDueDate { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "money")]
        public decimal DonationAmount { get; set; }
    }
}
