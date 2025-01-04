using Microsoft.EntityFrameworkCore;
using Mxg.Petid.ApiService.Net.Application.Specifications;
using Mxg.Petid.ApiService.Net.Domain.Common;

namespace Mxg.Petid.ApiService.Net.Infraestructure.Specification;

/// <summary>
/// Aqú se define la condición lógica.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SpecificationEvaluator<T> where T : BaseAuditableEntity//Solo se aplicara a las clases que hereden desde BaseAuditableEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        if (specification.Criteria != null)
        {
            inputQuery = inputQuery.Where(specification.Criteria);
        }

        if (specification.OrderBy != null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderBy);
        }

        if (specification.OrderByDescending != null)
        {
            inputQuery = inputQuery.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsPagingEnable)
        {
            inputQuery = inputQuery.Skip(specification.Skip).Take(specification.Take);
        }

        inputQuery = specification.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

        if (specification.IncludeStrings?.Any() == true) // ChatGPT
        {
            inputQuery = specification.IncludeStrings.Aggregate(inputQuery, (current, include) => current.Include(include));
        }

        return inputQuery;
    }
}