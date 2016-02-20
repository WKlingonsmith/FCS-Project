using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class PatientProblemMap : EntityTypeConfiguration<PatientProblem>
    {
        public PatientProblemMap()
        {
            // Primary Key
            this.HasKey(t => t.PatientProblemID);

            // Properties
            this.Property(t => t.ProbelmType)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PatientProblem");
            this.Property(t => t.PatientProblemID).HasColumnName("PatientProblemID");
            this.Property(t => t.PatientID).HasColumnName("PatientID");
            this.Property(t => t.ProblemID).HasColumnName("ProblemID");
            this.Property(t => t.ProbelmType).HasColumnName("ProbelmType");

            // Relationships
            this.HasRequired(t => t.Patient)
                .WithMany(t => t.PatientProblems)
                .HasForeignKey(d => d.PatientID);
            this.HasRequired(t => t.Problem)
                .WithMany(t => t.PatientProblems)
                .HasForeignKey(d => d.ProblemID);

        }
    }
}
