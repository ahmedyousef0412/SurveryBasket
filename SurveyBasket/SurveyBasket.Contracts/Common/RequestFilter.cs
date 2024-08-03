
namespace SurveyBasket.Contracts.Common;
public record RequestFilter
{

    private const int MaxPageSize = 15;

    private int _pageSize = 10;

    public int PageNumber { get; init; } = 1;


    public int PageSize
    {

        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;

    }
    public string? SearchValue { get; init; }
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; } = "ASC";
}
