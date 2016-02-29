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
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }

        public GrantsDataGrid(string name, decimal remain, DateTime date, string purp, string purpDes)
	    {
            GrantName = name;
            DonationAmount = remain;
            DonationDate = date;
            PurposeName = purp;
            PurposeDescription = purpDes;
	    }
        public GrantsDataGrid()
        { }
    }
}