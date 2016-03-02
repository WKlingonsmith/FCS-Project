using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class ProblemMap : EntityTypeConfiguration<Problem>
    {
        public ProblemMap()
        {
            // Primary Key
            this.HasKey(t => t.ProblemID);

            // Properties
            this.Property(t => t.ProblemType)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Problem");
            this.Property(t => t.ProblemID).HasColumnName("ProblemID");
            this.Property(t => t.ProblemType).HasColumnName("ProblemType");
        }
    }
}
