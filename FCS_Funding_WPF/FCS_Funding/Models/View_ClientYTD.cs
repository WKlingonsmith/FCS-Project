using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Funding.Models
{
    public partial class View_ClientYTD
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public int PatientName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        public string PatientOQ { get; set; }

        [Key]
        [Column(Order = 2)]
        [DataType(DataType.Currency)]
        public string PatientTotalCopay { get; set; }
        
    }
}


