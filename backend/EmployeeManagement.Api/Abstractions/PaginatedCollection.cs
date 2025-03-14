using System.Text.Json.Serialization;

namespace EmployeeManagement.Api.Abstractions;

public record PaginatedCollection<T> where T : class
{
  public PaginatedCollection(int page, int limit, int total, IEnumerable<T> items)
  {
    Page = page;
    Limit = limit;
    Total = total;
    Items = items;
  }
  [JsonPropertyOrder(1)]
  public int Page { get; }
  [JsonPropertyOrder(2)]
  public int Limit { get; }
  [JsonPropertyOrder(3)]
  public int Total { get; }
  [JsonPropertyOrder(4)]
  public IEnumerable<T> Items { get; } = Enumerable.Empty<T>();
}