using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class EventsDataGrid
    {
        // Getters and Setters for data grid values
        public int EventID { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }

        public EventsDataGrid()
        { }
        //Pass in values for Events Data Grid
        public EventsDataGrid(int eID, DateTime eStartDate, DateTime eEndDate, string eName, string eDescription)
        {
            EventID = eID;
            EventStartDateTime = eStartDate;
            EventEndDateTime = eEndDate;
            EventName = eName;
            EventDescription = eDescription;
        }
    }
}
