using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class AdminDataGrid
    {
        public int StaffID { get; set; }
        public string StaffUserName { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffTitle { get; set; }
        public string StaffDBRole { get; set; }



        public AdminDataGrid()
        { }

        public AdminDataGrid(int sID, string fName, string lName, string username, string title, string role)
        {
            StaffID = sID;
            StaffFirstName = fName;
            StaffLastName = lName;
            StaffUserName = username;
            StaffTitle = title;
            StaffDBRole = role;
        }
    }
}


