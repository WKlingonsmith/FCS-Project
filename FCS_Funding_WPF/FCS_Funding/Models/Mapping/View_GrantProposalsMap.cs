using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class View_GrantProposalsMap : EntityTypeConfiguration<View_GrantProposals>
    {
        public View_GrantProposalsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.GrantName, t.SubmissionDueDate, t.DonationAmount });

            // Properties
            this.Property(t => t.OrganizationName)
                .HasMaxLength(250);

            this.Property(t => t.GrantName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DonationAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_GrantProposals");
            this.Property(t => t.OrganizationName).HasColumnName("OrganizationName");
            this.Property(t => t.GrantName).HasColumnName("GrantName");
            this.Property(t => t.SubmissionDueDate).HasColumnName("SubmissionDueDate");
            this.Property(t => t.DonationAmount).HasColumnName("DonationAmount");
        }
    }
}
