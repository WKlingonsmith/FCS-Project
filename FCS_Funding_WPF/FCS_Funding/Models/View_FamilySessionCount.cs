using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class View_FamilySessionCount
    {
        public int StaffID { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public Nullable<int> Family_Sessions { get; set; }
    }
}
