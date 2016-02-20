using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class View_GroupSessionPreCountMap : EntityTypeConfiguration<View_GroupSessionPreCount>
    {
        public View_GroupSessionPreCountMap()
        {
            // Primary Key
            this.HasKey(t => new { t.StaffID, t.StaffFirstName, t.StaffLastName, t.AppointmentID });

            // Properties
            this.Property(t => t.StaffID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.StaffFirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StaffLastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AppointmentID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_GroupSessionPreCount");
            this.Property(t => t.StaffID).HasColumnName("StaffID");
            this.Property(t => t.StaffFirstName).HasColumnName("StaffFirstName");
            this.Property(t => t.StaffLastName).HasColumnName("StaffLastName");
            this.Property(t => t.AppointmentID).HasColumnName("AppointmentID");
        }
    }
}
