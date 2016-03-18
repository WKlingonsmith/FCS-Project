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
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double Length { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Value { get; set; }
        public string ServiceDescription { get; set; }
        //ID's
        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int ServiceID { get; set; }

        public InKindService()
        { }

        public InKindService(string dfn, string dln, DateTime sdt, DateTime edt, decimal rph, string sd, double length, decimal value)
        {
            decimal timeDiff = (decimal)(edt - sdt).TotalHours;
            DonorFirstName = dfn;
            DonorLastName = dln;
            StartDateTime = sdt;
            EndDateTime = edt;
            Length = length;
            RatePerHour = rph;
            Value = value;
            ServiceDescription = sd;
        }

    }

}