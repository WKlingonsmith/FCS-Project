namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FundRaisingEvent")]
    public partial class FundRaisingEvent
    {
        //constructor.  Made by Ken?  Dunno
        FundRaisingEvent(int eID, DateTime eStartDate, DateTime eEndDate, string eName, string eDescription)
        {
            EventID = eID;
            EventStartDateTime = eStartDate;
            EventEndDateTime = eEndDate;
            EventName = eName;
            EventDescription = eDescription;
        }

        [Key]
        public int EventID { get; set; }

        public DateTime EventStartDateTime { get; set; }

        public DateTime EventEndDateTime { get; set; }

        [Required]
        [StringLength(50)]
        public string EventName { get; set; }

        [StringLength(5000)]
        public string EventDescription { get; set; }
    }
}
