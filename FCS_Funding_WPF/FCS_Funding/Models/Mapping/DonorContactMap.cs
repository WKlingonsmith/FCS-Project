using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class DonorContactMap : EntityTypeConfiguration<DonorContact>
    {
        public DonorContactMap()
        {
            // Primary Key
            this.HasKey(t => t.ContactID);

            // Properties
            this.Property(t => t.ContactFirstName)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.ContactLastName)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.ContactPhone)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ContactEmail)
                .HasMaxLength(700);

            // Table & Column Mappings
            this.ToTable("DonorContact");
            this.Property(t => t.ContactID).HasColumnName("ContactID");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.ContactFirstName).HasColumnName("ContactFirstName");
            this.Property(t => t.ContactLastName).HasColumnName("ContactLastName");
            this.Property(t => t.ContactPhone).HasColumnName("ContactPhone");
            this.Property(t => t.ContactEmail).HasColumnName("ContactEmail");

            // Relationships
            this.HasRequired(t => t.Donor)
                .WithMany(t => t.DonorContacts)
                .HasForeignKey(d => d.DonorID);

        }
    }
}
