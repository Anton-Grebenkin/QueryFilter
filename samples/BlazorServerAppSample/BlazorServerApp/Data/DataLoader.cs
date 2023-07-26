using BlazorServerApp.Interfaces;
using QueryFilter.Extensions;
using QueryFilter.Models;

namespace BlazorServerApp.Data
{
    public class DataLoader : IDataLoader<WeatherForecast>
    {
        public async Task<(IEnumerable<WeatherForecast> items, int totalCount)> LoadDataAsync(Filter filter)
        {
            var result = Weather.Data.AsQueryable().GetQueryResult(filter);
            return (result.Items, result.TotalCount);
        }
    }
}
