using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class FundRaisingEvent
    {
        public FundRaisingEvent(DateTime start, DateTime end, string name, string des)
        {
            EventStartDateTime = start;
            EventEndDateTime = end;
            EventName = name;
            EventDescription = des;
        }

        public int EventID { get; set; }
        public System.DateTime EventStartDateTime { get; set; }
        public System.DateTime EventEndDateTime { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
    }
}
