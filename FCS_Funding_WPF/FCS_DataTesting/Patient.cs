using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class Patient
    {
        public int PatientOQ { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AgeGroup { get; set; }
        public string Ethnicity { get; set; }
        public DateTime Time { get; set; }
        public Boolean IsHead { get; set; }
        public string RelationToHead { get;  set; }

        public Patient(int p, string fn, string ln, string g, string ag, string e, DateTime t, Boolean h, string rth)
        {
            PatientOQ = p;
            FirstName = fn;
            LastName = ln;
            Gender = g;
            AgeGroup = ag;
            Ethnicity = e;
            Time = t;
            IsHead = h;
            RelationToHead = rth;
        }
    }
}
