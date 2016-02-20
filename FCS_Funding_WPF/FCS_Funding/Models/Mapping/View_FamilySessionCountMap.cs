using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class View_FamilySessionCountMap : EntityTypeConfiguration<View_FamilySessionCount>
    {
        public View_FamilySessionCountMap()
        {
            // Primary Key
            this.HasKey(t => new { t.StaffID, t.StaffFirstName, t.StaffLastName });

            // Properties
            this.Property(t => t.StaffID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.StaffFirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StaffLastName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_FamilySessionCount");
            this.Property(t => t.StaffID).HasColumnName("StaffID");
            this.Property(t => t.StaffFirstName).HasColumnName("StaffFirstName");
            this.Property(t => t.StaffLastName).HasColumnName("StaffLastName");
            this.Property(t => t.Family_Sessions).HasColumnName("Family Sessions");
        }
    }
}
