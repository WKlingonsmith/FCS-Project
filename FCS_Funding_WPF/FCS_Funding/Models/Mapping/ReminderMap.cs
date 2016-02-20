using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class ReminderMap : EntityTypeConfiguration<Reminder>
    {
        public ReminderMap()
        {
            // Primary Key
            this.HasKey(t => t.ReminderID);

            // Properties
            this.Property(t => t.ReminderDescription)
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("Reminder");
            this.Property(t => t.ReminderID).HasColumnName("ReminderID");
            this.Property(t => t.DonorID).HasColumnName("DonorID");
            this.Property(t => t.ReminderDate).HasColumnName("ReminderDate");
            this.Property(t => t.ReminderDescription).HasColumnName("ReminderDescription");

            // Relationships
            this.HasRequired(t => t.Donor)
                .WithMany(t => t.Reminders)
                .HasForeignKey(d => d.DonorID);

        }
    }
}
