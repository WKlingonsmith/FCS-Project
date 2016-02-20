using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class PatientMap : EntityTypeConfiguration<Patient>
    {
        public PatientMap()
        {
            // Primary Key
            this.HasKey(t => t.PatientID);

            // Properties
            this.Property(t => t.PatientFirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PatientLastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PatientGender)
                .HasMaxLength(6);

            this.Property(t => t.PatientAgeGroup)
                .HasMaxLength(6);

            this.Property(t => t.PatientEthnicity)
                .HasMaxLength(50);

            this.Property(t => t.RelationToHead)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Patient");
            this.Property(t => t.PatientID).HasColumnName("PatientID");
            this.Property(t => t.PatientOQ).HasColumnName("PatientOQ");
            this.Property(t => t.HouseholdID).HasColumnName("HouseholdID");
            this.Property(t => t.PatientFirstName).HasColumnName("PatientFirstName");
            this.Property(t => t.PatientLastName).HasColumnName("PatientLastName");
            this.Property(t => t.PatientGender).HasColumnName("PatientGender");
            this.Property(t => t.PatientAgeGroup).HasColumnName("PatientAgeGroup");
            this.Property(t => t.PatientEthnicity).HasColumnName("PatientEthnicity");
            this.Property(t => t.NewClientIntakeHour).HasColumnName("NewClientIntakeHour");
            this.Property(t => t.IsHead).HasColumnName("IsHead");
            this.Property(t => t.RelationToHead).HasColumnName("RelationToHead");

            // Relationships
            this.HasRequired(t => t.PatientHousehold)
                .WithMany(t => t.Patients)
                .HasForeignKey(d => d.HouseholdID);

        }
    }
}
