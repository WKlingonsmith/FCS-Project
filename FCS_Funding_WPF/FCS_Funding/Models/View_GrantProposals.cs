using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class View_GrantProposals
    {
        public string OrganizationName { get; set; }
        public string GrantName { get; set; }
        public System.DateTime SubmissionDueDate { get; set; }
        public decimal DonationAmount { get; set; }
    }
}
