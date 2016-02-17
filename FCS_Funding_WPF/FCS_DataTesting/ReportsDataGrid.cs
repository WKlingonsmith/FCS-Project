using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class ReportsDataGrid
    {
        // Getters and Setters for data grid values
        public string EventName { get; set; }
        public string EventDescription { get; set; }

        public ReportsDataGrid(string eName, string eDesc)
        {
            EventDescription = eDesc;
            EventName = eName;

        }
    }
}
