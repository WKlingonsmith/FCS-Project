using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class AdminDataGrid
    {
        public string StaffID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AdminDataGrid(string sID, string fName, string lName)
        {
            StaffID = sID;
            FirstName = fName;
            LastName = lName;
        }
    }
}