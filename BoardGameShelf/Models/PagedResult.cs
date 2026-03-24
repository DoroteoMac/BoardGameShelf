namespace BoardGameShelf.Models;

/// <summary>
/// Represents a paginated result set.
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// The items in the current page.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// The maximum number of items returned in this page.
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// The number of items skipped before this page.
    /// </summary>
    public int Offset { get; set; }
}
