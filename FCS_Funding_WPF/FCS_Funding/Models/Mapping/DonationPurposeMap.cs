using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class DonationPurposeMap : EntityTypeConfiguration<DonationPurpose>
    {
        public DonationPurposeMap()
        {
            // Primary Key
            this.HasKey(t => t.DonationPurposeID);

            // Properties
            // Table & Column Mappings
            this.ToTable("DonationPurpose");
            this.Property(t => t.DonationPurposeID).HasColumnName("DonationPurposeID");
            this.Property(t => t.DonationID).HasColumnName("DonationID");
            this.Property(t => t.PurposeID).HasColumnName("PurposeID");
            this.Property(t => t.DonationPurposeAmount).HasColumnName("DonationPurposeAmount");

            // Relationships
            this.HasRequired(t => t.Donation)
                .WithMany(t => t.DonationPurposes)
                .HasForeignKey(d => d.DonationID);
            this.HasRequired(t => t.Purpose)
                .WithMany(t => t.DonationPurposes)
                .HasForeignKey(d => d.PurposeID);

        }
    }
}
