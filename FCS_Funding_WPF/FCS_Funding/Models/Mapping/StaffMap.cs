using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class StaffMap : EntityTypeConfiguration<Staff>
    {
        public StaffMap()
        {
            // Primary Key
            this.HasKey(t => t.StaffID);

            // Properties
            this.Property(t => t.StaffFirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StaffLastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StaffTitle)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StaffUserName)
                .HasMaxLength(50);

            this.Property(t => t.StaffPassword)
                .HasMaxLength(50);

            this.Property(t => t.StaffDBRole)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Staff");
            this.Property(t => t.StaffID).HasColumnName("StaffID");
            this.Property(t => t.StaffFirstName).HasColumnName("StaffFirstName");
            this.Property(t => t.StaffLastName).HasColumnName("StaffLastName");
            this.Property(t => t.StaffTitle).HasColumnName("StaffTitle");
            this.Property(t => t.StaffUserName).HasColumnName("StaffUserName");
            this.Property(t => t.StaffPassword).HasColumnName("StaffPassword");
            this.Property(t => t.StaffDBRole).HasColumnName("StaffDBRole");
        }
    }
}
