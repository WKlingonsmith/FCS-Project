using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class PatientProblem
    {
        public int PatientProblemID { get; set; }
        public int PatientID { get; set; }
        public int ProblemID { get; set; }
        public string ProbelmType { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Problem Problem { get; set; }
    }
}
