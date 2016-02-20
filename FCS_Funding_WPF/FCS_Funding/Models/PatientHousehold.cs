using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class PatientHousehold
    {
        public PatientHousehold(int hpop, string inc, string houseCounty)
        {
            HouseholdPopulation = hpop;
            HouseholdIncomeBracket = inc;
            HouseholdCounty = houseCounty;
            this.Patients = new List<Patient>();
        }

        public int HouseholdID { get; set; }
        public int HouseholdPopulation { get; set; }
        public string HouseholdIncomeBracket { get; set; }
        public string HouseholdCounty { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
