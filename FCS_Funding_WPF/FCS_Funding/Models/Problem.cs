using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Problem
    {
        public Problem()
        {
            this.PatientProblems = new List<PatientProblem>();
        }

        public int ProblemID { get; set; }
        public string ProblemType { get; set; }
        public virtual ICollection<PatientProblem> PatientProblems { get; set; }
    }
}
