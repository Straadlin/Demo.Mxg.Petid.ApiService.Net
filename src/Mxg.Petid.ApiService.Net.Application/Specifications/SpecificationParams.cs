namespace Mxg.Petid.ApiService.Net.Application.Specifications;

/// <summary>
/// Specification params base.
/// </summary>
public abstract class SpecificationParams
{
    public string? Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    private const int MaxPageSize = 50;
    private int pageSize = 3;

    public int PageSize
    {
        get => this.pageSize;
        set => this.pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? Search { get; set; }
}