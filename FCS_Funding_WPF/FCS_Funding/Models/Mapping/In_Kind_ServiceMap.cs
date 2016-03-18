using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class In_Kind_ServiceMap : EntityTypeConfiguration<In_Kind_Service>
    {
        public In_Kind_ServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceID);

            // Properties
            this.Property(t => t.ServiceDescription)
                .IsRequired()
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("In_Kind_Service");
            this.Property(t => t.ServiceID).HasColumnName("ServiceID");
            this.Property(t => t.DonationID).HasColumnName("DonationID");
            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");
            this.Property(t => t.EndDateTime).HasColumnName("EndDateTime");
            this.Property(t => t.RatePerHour).HasColumnName("RatePerHour");
            this.Property(t => t.ServiceDescription).HasColumnName("ServiceDescription");
            this.Property(t => t.ServiceLength).HasColumnName("ServiceLength");
            this.Property(t => t.ServiceValue).HasColumnName("ServiceValue");

            // Relationships
            this.HasRequired(t => t.Donation)
                .WithMany(t => t.In_Kind_Service)
                .HasForeignKey(d => d.DonationID);

        }
    }
}
