﻿using Employee.Domain.Models;
using Employee.Domain.Pagination;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;
using Employee.Infra.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infra.EFCore.Repositories;

public class EmployeeRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, EmployeeModel, Guid>(context), IEmployeeRepository
{
    public async Task<bool> NotExist(string document, CancellationToken cancellationToken = default)
    {
        return !await Context.Employees.AnyAsync(m => m.Document == document, cancellationToken);
    }

    public async Task<PagedResult<EmployeeModel>> GetPaged(string email, string document, int page, int pageSize)
    {
        IQueryable<EmployeeModel> query = Context.Employees;

        if (!string.IsNullOrEmpty(email))
            query = query.Where(e => e.Email == email);

        if (!string.IsNullOrEmpty(document))
            query = query.Where(e => e.Document == document);

        return await query.GetPaged(page, pageSize);
    }
}