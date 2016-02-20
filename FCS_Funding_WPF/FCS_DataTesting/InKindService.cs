using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class InKindService
    {
        public string DonorFirstName  { get; set;}
        public string DonorLastName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal Length { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Value { get; set; }
        public string ServiceDescription { get; set;}

        public InKindService(string dfn, string dln, string o, DateTime sdt, DateTime edt, decimal rph, string sd)
        {
            decimal timeDiff = (decimal)(edt - sdt).TotalHours;
            DonorFirstName = dfn;
            DonorLastName = dln;
            OrganizationName = o;
            StartDateTime = sdt;
            EndDateTime = edt;
            Length = Math.Round(timeDiff, 2);
            RatePerHour = rph;
            Value = Math.Round(rph * timeDiff, 2);
            ServiceDescription = sd;
        }

    }

}