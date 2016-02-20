using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class GrantProposalMap : EntityTypeConfiguration<GrantProposal>
    {
        public GrantProposalMap()
        {
            // Primary Key
            this.HasKey(t => t.GrantProposalID);

            // Properties
            this.Property(t => t.GrantName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("GrantProposal");
            this.Property(t => t.GrantProposalID).HasColumnName("GrantProposalID");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.GrantName).HasColumnName("GrantName");
            this.Property(t => t.SubmissionDueDate).HasColumnName("SubmissionDueDate");

            // Relationships
            this.HasRequired(t => t.Donor)
                .WithMany(t => t.GrantProposals)
                .HasForeignKey(d => d.DonorID);

        }
    }
}
