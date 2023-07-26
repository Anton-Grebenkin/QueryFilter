using QueryFilter.Models;

namespace BlazorServerApp.Interfaces
{
    public interface IDataLoader<T>
    {
        Task<(IEnumerable<T> items, int totalCount)> LoadDataAsync(Filter filter);
    }
}
