using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FCS_Funding.Models.Mapping
{
    public class ExpenseTypeMap : EntityTypeConfiguration<ExpenseType>
    {
        public ExpenseTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ExpenseTypeID);

            // Properties
            this.Property(t => t.ExpenseType1)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ExpenseDescription)
                .HasMaxLength(5000);

            // Table & Column Mappings
            this.ToTable("ExpenseType");
            this.Property(t => t.ExpenseTypeID).HasColumnName("ExpenseTypeID");
            this.Property(t => t.ExpenseType1).HasColumnName("ExpenseType");
            this.Property(t => t.ExpenseDescription).HasColumnName("ExpenseDescription");
        }
    }
}
