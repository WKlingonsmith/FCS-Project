using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class PurposeMap : EntityTypeConfiguration<Purpose>
    {
        public PurposeMap()
        {
            // Primary Key
            this.HasKey(t => t.PurposeID);

            // Properties
            this.Property(t => t.PurposeName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PurposeDescription)
                .IsRequired()
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("Purpose");
            this.Property(t => t.PurposeID).HasColumnName("PurposeID");
            this.Property(t => t.PurposeName).HasColumnName("PurposeName");
            this.Property(t => t.PurposeDescription).HasColumnName("PurposeDescription");
        }
    }
}
