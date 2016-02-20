using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class ExpenseMap : EntityTypeConfiguration<Expense>
    {
        public ExpenseMap()
        {
            // Primary Key
            this.HasKey(t => t.ExpenseID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Expense");
            this.Property(t => t.ExpenseID).HasColumnName("ExpenseID");
            this.Property(t => t.ExpenseTypeID).HasColumnName("ExpenseTypeID");
            this.Property(t => t.DonationID).HasColumnName("DonationID");
            this.Property(t => t.PatientID).HasColumnName("PatientID");
            this.Property(t => t.AppointmentID).HasColumnName("AppointmentID");
            this.Property(t => t.ExpenseDueDate).HasColumnName("ExpenseDueDate");
            this.Property(t => t.ExpensePaidDate).HasColumnName("ExpensePaidDate");
            this.Property(t => t.ExpenseAmount).HasColumnName("ExpenseAmount");

            // Relationships
            this.HasOptional(t => t.Appointment)
                .WithMany(t => t.Expenses)
                .HasForeignKey(d => d.AppointmentID);
            this.HasOptional(t => t.Donation)
                .WithMany(t => t.Expenses)
                .HasForeignKey(d => d.DonationID);
            this.HasRequired(t => t.ExpenseType)
                .WithMany(t => t.Expenses)
                .HasForeignKey(d => d.ExpenseTypeID);
            this.HasOptional(t => t.Patient)
                .WithMany(t => t.Expenses)
                .HasForeignKey(d => d.PatientID);

        }
    }
}
