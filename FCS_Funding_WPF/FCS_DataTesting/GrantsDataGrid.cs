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
        public decimal AmountRemaining { get; set; }
        public DateTime DateReceived { get; set; }
        public string Purpose { get; set; }
        public string PurposeDescription { get; set; }

        public GrantsDataGrid(string name, decimal remain, DateTime date, string purp, string purpDes)
	    {
            GrantName = name;
            AmountRemaining = remain;
            DateReceived = date;
            Purpose = purp;
            PurposeDescription = purpDes;
	    }
    }
}