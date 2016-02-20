using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class In_Kind_ItemMap : EntityTypeConfiguration<In_Kind_Item>
    {
        public In_Kind_ItemMap()
        {
            // Primary Key
            this.HasKey(t => t.ItemID);

            // Properties
            this.Property(t => t.ItemName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ItemDescription)
                .IsRequired()
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("In_Kind_Item");
            this.Property(t => t.ItemID).HasColumnName("ItemID");
            this.Property(t => t.DonationID).HasColumnName("DonationID");
            this.Property(t => t.ItemName).HasColumnName("ItemName");
            this.Property(t => t.ItemDescription).HasColumnName("ItemDescription");

            // Relationships
            this.HasRequired(t => t.Donation)
                .WithMany(t => t.In_Kind_Item)
                .HasForeignKey(d => d.DonationID);

        }
    }
}
