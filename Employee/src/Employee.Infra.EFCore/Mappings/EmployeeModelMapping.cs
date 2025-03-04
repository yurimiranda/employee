using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee.Infra.EFCore.Mappings
{
    public class EmployeeModelMapping : IEntityTypeConfiguration<EmployeeModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeModel> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Document)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.ImmediateSupervisor)
                .HasMaxLength(100);

            builder.Property(e => e.BirthDate)
                .IsRequired();

            builder.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Phones)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Employee");
        }
    }
}