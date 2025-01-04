using System.Linq.Expressions;

namespace Mxg.Petid.ApiService.Net.Application.Specifications;

/// <summary>
/// Specification base.
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification()
    {
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria { get; }

    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    public List<string> IncludeStrings { get; } = new List<string>(); // ChatGPT

    public Expression<Func<T, object>> OrderBy { get; private set; }

    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    //---------------------------------------------------------------------------

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnable = true;
    }

    public bool IsPagingEnable { get; private set; }

    protected void AddIncluded(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString) // ChatGPT
    {
        IncludeStrings.Add(includeString);
    }
}