﻿using MagicVilla_API.Data;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        this.dbSet = _dbContext.Set<T>();
    }

    public async Task Create(T entity)
    {
        await dbSet.AddAsync(entity);
        await Save();
    }

    public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true)
    {
        IQueryable<T> query = dbSet;

        if (!tracked) query = query.AsNoTracking();

        if (filter != null) query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null) query = query.Where(filter);

        return await query.ToListAsync();
    }

    public async Task Remove(T entity)
    {
        dbSet.Remove(entity);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
