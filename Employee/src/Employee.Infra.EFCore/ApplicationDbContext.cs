﻿using Employee.Domain.Abstractions.Interfaces;
using Employee.Infra.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace Employee.Infra.EFCore;

public partial class ApplicationDbContext : DbContext, ISecurityKeyContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<KeyMaterial> SecurityKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(type => type.ClrType.GetInterfaces().Contains(typeof(IEntity)));

        foreach (var type in entityTypes)
        {
            modelBuilder.SetSoftDeleteFilter(type.ClrType);
        }

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}