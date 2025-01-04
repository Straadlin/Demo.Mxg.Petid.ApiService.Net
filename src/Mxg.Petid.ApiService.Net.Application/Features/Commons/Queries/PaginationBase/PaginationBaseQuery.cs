namespace Mxg.Petid.ApiService.Net.Application.Features.Commons.Queries.PaginationBase;

/// <summary>
/// Query for pagination.
/// </summary>
public class PaginationBaseQuery
{
    private int pageSize = 10;
    private const int MaxPageSize = 50;

    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    //public int PageSize { get; set; }
    public int PageSize
    {
        get => this.pageSize;
        set => this.pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}