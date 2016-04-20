using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class DonationsGrid
    {
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }
        public int DonorID { get; set; }
        public int DonationPurposeID { get; set; }
        public int PurposeID { get; set; }
        public int DonationID { get; set; }
        public decimal DonationAmountRemaining { get; set; }

        //nullable variables
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string OrganizationName { get; set; }
        public DonationsGrid()
        { }
    }
}