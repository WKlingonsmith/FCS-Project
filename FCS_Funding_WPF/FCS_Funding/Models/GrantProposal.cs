namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrantProposal")]
    public partial class GrantProposal
    {
        //Constructor made by Ken
        public GrantProposal(int gpID, int dID, String gName, DateTime sdDate)
        {
            GrantProposalID = gpID;
            DonorID = dID;
            GrantName = gName;
            SubmissionDueDate = sdDate;
        }

        public int GrantProposalID { get; set; }

        public int DonorID { get; set; }

        [Required]
        [StringLength(50)]
        public string GrantName { get; set; }

        [Column(TypeName = "date")]
        public DateTime SubmissionDueDate { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
