using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class DonationMap : EntityTypeConfiguration<Donation>
    {
        public DonationMap()
        {
            // Primary Key
            this.HasKey(t => t.DonationID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Donation");
            this.Property(t => t.DonationID).HasColumnName("DonationID");
            this.Property(t => t.EventID).HasColumnName("EventID");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.GrantProposalID).HasColumnName("GrantProposalID");
            this.Property(t => t.Restricted).HasColumnName("Restricted");
            this.Property(t => t.InKind).HasColumnName("InKind");
            this.Property(t => t.DonationAmount).HasColumnName("DonationAmount");
            this.Property(t => t.DonationDate).HasColumnName("DonationDate");

            // Relationships
            this.HasRequired(t => t.Donor)
                .WithMany(t => t.Donations)
                .HasForeignKey(d => d.DonorID);

        }
    }
}
