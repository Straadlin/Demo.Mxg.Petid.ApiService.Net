﻿using Mxg.Petid.ApiService.Net.Application.Specifications;
using Mxg.Petid.ApiService.Net.Domain.Common;
using System.Linq.Expressions;

namespace Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;

public interface IAsyncRepository<T> where T : BaseAuditableEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                    string includeString = null,
                                    bool disableTracking = true);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                    List<Expression<Func<T, object>>> includes = null,
                                    bool disableTracking = true);

    Task<T> GetByIdAsync(long id);

    //Task<T> AddAsync(T entity);
    //Task<T> UpdateAsync(T entity);
    //Task DeleteAsync(T entity);

    void AddEntity(T entity);
    void UpdateEntity(T entity);
    void DeleteEntity(T entity);

    Task<T> GetByIdWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
}