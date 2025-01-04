using System.Linq.Expressions;

namespace Mxg.Petid.ApiService.Net.Application.Specifications;

/// <summary>
/// Specification contract.
/// </summary>
/// <typeparam name="T">Generic type</typeparam>
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; } // ChatGPT

    //---------------------------------------------------------------------------

    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnable { get; }
}