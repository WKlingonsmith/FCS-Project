using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class FundRaisingEventMap : EntityTypeConfiguration<FundRaisingEvent>
    {
        public FundRaisingEventMap()
        {
            // Primary Key
            this.HasKey(t => t.EventID);

            // Properties
            this.Property(t => t.EventName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EventDescription)
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("FundRaisingEvent");
            this.Property(t => t.EventID).HasColumnName("EventID");
            this.Property(t => t.EventStartDateTime).HasColumnName("EventStartDateTime");
            this.Property(t => t.EventEndDateTime).HasColumnName("EventEndDateTime");
            this.Property(t => t.EventName).HasColumnName("EventName");
            this.Property(t => t.EventDescription).HasColumnName("EventDescription");
        }
    }
}
