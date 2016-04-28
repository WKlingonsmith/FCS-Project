using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class Purposes
    {
        public int DonationID
        {
            get; set;
        }
       public DateTime? DonationExpirationDate { get; set; }
       public bool Restricted { get; set; }
       public int PurposeID { get; set; }
    }
}
