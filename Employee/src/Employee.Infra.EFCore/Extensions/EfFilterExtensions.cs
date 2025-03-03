using Employee.Domain.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Employee.Infra.EFCore.Extensions;

public static class EfFilterExtensions
{
    public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
    {
        SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
            .Invoke(null, [modelBuilder]);
    }

    static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EfFilterExtensions)
               .GetMethods(BindingFlags.Public | BindingFlags.Static)
               .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");

    public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
        where TEntity : class, IEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.Active);
    }
}