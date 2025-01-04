using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Application.Common.Interfaces.Persistance;
using Mxg.Petid.ApiService.Net.Application.Specifications;
using Mxg.Petid.ApiService.Net.Domain.Common;
using Mxg.Petid.ApiService.Net.Infraestructure.Specification;
using System.Linq.Expressions;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Persistance.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseAuditableEntity
{
    protected readonly PetidDbContext context;

    public RepositoryBase(PetidDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await this.context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Find a resource with parameters "disableTracking = false".
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await this.context.Set<T>().Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Este método utiliza una cadena de inclusión (includeString) para cargar propiedades relacionadas. 
    /// Es útil cuando solo necesitas incluir una o dos propiedades relacionadas en la consulta. 
    /// Sin embargo, este enfoque tiene limitaciones en la profundidad de las relaciones que puedes cargar y 
    /// no te permite especificar cómo se deben ordenar las entidades relacionadas.
    /// Solo permite hacer consulta (join) sobre 2 entidades.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeString">Lazi Loading(Carga Diferida/Perezosa)</param>
    /// <param name="disableTracking"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                 string includeString = null,
                                                 bool disableTracking = true)
    {
        IQueryable<T> query = this.context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    /// <summary>
    /// Este método utiliza una lista de expresiones lambda para incluir propiedades relacionadas. 
    /// Esto te brinda más flexibilidad y control sobre qué propiedades incluir y te permite cargar relaciones 
    /// de manera más profunda. Además, puedes especificar múltiples propiedades relacionadas para incluir en la consulta. 
    /// También te permite ordenar los resultados y especificar el orden en que se incluyen las propiedades relacionadas.
    /// Este método tiliza la lista de expresiones lambda (includes). 
    /// Esto te proporciona más control y flexibilidad para manejar consultas complejas con múltiples joins. 
    /// Además, te permite incluir múltiples propiedades relacionadas en la consulta y especificar cómo deben ordenarse los resultados.
    /// Este método es más versátil y adecuado para consultas que involucran relaciones complejas entre múltiples tablas. 
    /// Sin embargo, elige el método que mejor se adapte a tus necesidades y a la complejidad de tus consultas.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="includes">Eager Loading (Carga Anticipada/Ansiosa)</param>
    /// <param name="disableTracking"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                 List<Expression<Func<T, object>>> includes = null,
                                                 bool disableTracking = true)
    {
        IQueryable<T> query = this.context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(long id)
    {
        return await this.context.Set<T>().FindAsync(id);
    }

    //public async Task<T> AddAsync(T entity)
    //{
    //    this.context.Set<T>().Add(entity);
    //    await this.context.SaveChangesAsync();
    //    return entity;
    //}

    //public async Task<T> UpdateAsync(T entity)
    //{
    //    this.context.Entry(entity).State = EntityState.Modified;
    //    await this.context.SaveChangesAsync();
    //    return entity;
    //}

    //public async Task DeleteAsync(T entity)
    //{
    //    this.context.Set<T>().Remove(entity);
    //    await this.context.SaveChangesAsync();
    //}

    public void AddEntity(T entity)
    {
        this.context.Set<T>().Add(entity);
    }

    public void UpdateEntity(T entity)
    {
        this.context.Set<T>().Attach(entity);
        this.context.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteEntity(T entity)
    {
        this.context.Set<T>().Remove(entity);
    }

    //----------------------------------------------------

    public async Task<T> GetByIdWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).CountAsync();
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(this.context.Set<T>().AsQueryable(), specification);
    }
}