using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class DonorMap : EntityTypeConfiguration<Donor>
    {
        public DonorMap()
        {
            // Primary Key
            this.HasKey(t => t.DonorID);

            // Properties
            this.Property(t => t.DonorType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OrganizationName)
                .HasMaxLength(250);

            this.Property(t => t.DonorAddress1)
                .HasMaxLength(50);

            this.Property(t => t.DonorAddress2)
                .HasMaxLength(50);

            this.Property(t => t.DonorState)
                .HasMaxLength(2);

            this.Property(t => t.DonorCity)
                .HasMaxLength(200);

            this.Property(t => t.DonorZip)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Donor");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.DonorType).HasColumnName("DonorType");
            this.Property(t => t.OrganizationName).HasColumnName("OrganizationName");
            this.Property(t => t.DonorAddress1).HasColumnName("DonorAddress1");
            this.Property(t => t.DonorAddress2).HasColumnName("DonorAddress2");
            this.Property(t => t.DonorState).HasColumnName("DonorState");
            this.Property(t => t.DonorCity).HasColumnName("DonorCity");
            this.Property(t => t.DonorZip).HasColumnName("DonorZip");
        }
    }
}
