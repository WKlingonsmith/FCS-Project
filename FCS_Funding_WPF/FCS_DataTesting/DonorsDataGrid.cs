using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class DonorsDataGrid
    {
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string OrganizationName { get; set; }
        public string DonorType { get; set; }
        public string DonorAddress1 { get; set; }
        public string DonorAddress2 { get; set; }
        public string DonorState { get; set; }
        public string DonorCity { get; set; }
        public string DonorZip { get; set; }
        public DonorsDataGrid(string fn, string ln, string on, string dt, string da1, string da2, string s, string c, string z)
        {
            DonorFirstName = fn;
            DonorLastName  = ln;
            OrganizationName = on;
            DonorType = dt;
            DonorAddress1 = da1;  
            DonorAddress2  = da2;
            DonorState = s;
            DonorCity = c;
            DonorZip  = z;
        }
    }
}
