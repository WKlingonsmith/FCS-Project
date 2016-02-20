using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class PatientHouseholdMap : EntityTypeConfiguration<PatientHousehold>
    {
        public PatientHouseholdMap()
        {
            // Primary Key
            this.HasKey(t => t.HouseholdID);

            // Properties
            this.Property(t => t.HouseholdIncomeBracket)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.HouseholdCounty)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PatientHousehold");
            this.Property(t => t.HouseholdID).HasColumnName("HouseholdID");
            this.Property(t => t.HouseholdPopulation).HasColumnName("HouseholdPopulation");
            this.Property(t => t.HouseholdIncomeBracket).HasColumnName("HouseholdIncomeBracket");
            this.Property(t => t.HouseholdCounty).HasColumnName("HouseholdCounty");
        }
    }
}
