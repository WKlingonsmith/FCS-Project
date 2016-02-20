using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class GrantProposal
    {
        public int GrantProposalID { get; set; }
        public int DonorID { get; set; }
        public string GrantName { get; set; }
        public System.DateTime SubmissionDueDate { get; set; }
        public virtual Donor Donor { get; set; }
    }
}
