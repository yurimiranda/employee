using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee.Infra.EFCore.Mappings
{
    public class PositionModelMapping : IEntityTypeConfiguration<PositionModel>
    {
        public void Configure(EntityTypeBuilder<PositionModel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.ToTable("Position");
        }
    }
}