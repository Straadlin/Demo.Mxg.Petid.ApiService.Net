namespace Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase.Dtos;

/// <summary>
/// Dto for pagination.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginationDto<T> where T : class
{
    public int Count { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyList<T>? Records { get; set; }
    public int PageCount { get; set; }
    public bool Any() => Count > 0;
}