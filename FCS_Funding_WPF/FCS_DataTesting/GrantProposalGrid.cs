using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class GrantProposalGrid
    {
        public string GrantName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime SubmissionDueDate { get; set; }
        public string GrantStatus { get; set; }
        public int GrantProposalID { get; set; }
        public int DonorID { get; set; }
        public GrantProposalGrid(string gName, string oName, DateTime sDate, string gStatus)
        {
            GrantName = gName;
            OrganizationName = oName;
            SubmissionDueDate = sDate;
            GrantStatus = gStatus;
        }
        public GrantProposalGrid()
        {
        }
    }
}
