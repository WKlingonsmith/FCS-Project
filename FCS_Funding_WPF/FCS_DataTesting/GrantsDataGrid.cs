using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class GrantsDataGrid
    {
        public string GrantName { get; set; }
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }
        public decimal DonationAmountRemaining { get; set; }

        public int PurposeID { get; set; }
        public int DonationID { get; set; }
        public int DonorID { get; set; }
        public int GrantProposalID { get; set; }

        public GrantsDataGrid(string name, decimal remain, DateTime date, DateTime exdate, string purp, string purpDes, decimal donamountremain)
	    {
            GrantName = name;
            DonationAmount = remain;
            DonationDate = date;
            PurposeName = purp;
            PurposeDescription = purpDes;
            DonationAmountRemaining = donamountremain;
            ExpirationDate = exdate;
        }
        public GrantsDataGrid()
        { }
    }
}