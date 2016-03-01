using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class DonorContactGrid
    {                        
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; } 
        public string ContactEmail { get; set; }
        public int DonorID { get; set; }
        public int ContactID { get; set; }
        //public DonorContactGrid(string fn, string ln, string p, string e)
        //{
        //    ContactFirstName = fn;
        //    ContactLastName = ln;
        //    ContactPhone = p;
        //    ContactEmail = e;
        //}
    };
}

