using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee.Infra.EFCore.Mappings
{
    public class PhoneModelMapping : IEntityTypeConfiguration<PhoneModel>
    {
        public void Configure(EntityTypeBuilder<PhoneModel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AreaCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(p => p.Number)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.IsPrimary)
                .IsRequired();

            builder.HasOne(p => p.Employee)
                .WithMany(e => e.Phones)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Phone");
        }
    }
}
