using BlazorServerApp.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorServerApp.Interfaces
{
    public interface ITable
    {
        public int PageSize { get;}

        public Task SetPageSizeAsync(int pageSize);

        void Refresh();

        Task UpdateAsync();

        int PageNumber { get; }

        int TotalCount { get; }

        public int TotalPages { get; }

        Task FirstPageAsync();
        Task NextPageAsync();
        Task PreviousPageAsync();
        Task LastPageAsync();

    }
}
