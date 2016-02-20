using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class AppointmentMap : EntityTypeConfiguration<Appointment>
    {
        public AppointmentMap()
        {
            // Primary Key
            this.HasKey(t => t.AppointmentID);

            // Properties
            this.Property(t => t.AppointmentCancelationType)
                .HasMaxLength(12);

            // Table & Column Mappings
            this.ToTable("Appointment");
            this.Property(t => t.AppointmentID).HasColumnName("AppointmentID");
            this.Property(t => t.StaffID).HasColumnName("StaffID");
            this.Property(t => t.AppointmentStartDate).HasColumnName("AppointmentStartDate");
            this.Property(t => t.AppointmentEndDate).HasColumnName("AppointmentEndDate");
            this.Property(t => t.AppointmentCancelationType).HasColumnName("AppointmentCancelationType");

            // Relationships
            this.HasRequired(t => t.Staff)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.StaffID);

        }
    }
}
