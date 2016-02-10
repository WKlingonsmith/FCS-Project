using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class Patient
    {
        public string Name { get; private set; }
        public int PatientOQ { get; private set; }
        public string Notes { get; private set; }

        public Patient(string n, int p, string note)
        {
            Name = n;
            PatientOQ = p;
            Notes = note;
        }
    }
}
