using BlazorServerApp.Interfaces;
using Microsoft.AspNetCore.Components;
using QueryFilter;
using QueryFilter.Models;
using System.Text;

namespace BlazorServerApp.Components
{
    public partial class Table<TableItem> : ITable
    {
        private const int DEFAULT_PAGE_SIZE = 10;
        public IEnumerable<TableItem> Items { get; set; } = new List<TableItem>();

        [Parameter]
        public IDataLoader<TableItem> DataLoader { get; set; }

        [Parameter]
        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

        public List<Column<TableItem>> Columns { get; } = new List<Column<TableItem>>();

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public int PageNumber { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages => PageSize <= 0 ? 1 : (TotalCount + PageSize - 1) / PageSize;

        public void AddColumn(Column<TableItem> column)
        {
            column.Table = this;

            if (column.Type == null)
            {
                column.Type = column.Field?.GetPropertyMemberInfo().GetMemberUnderlyingType();
            }

            Columns.Add(column);
            Refresh();
        }

        protected override async void OnInitialized()
        {
            await UpdateAsync();
        }

        public async Task SetPageSizeAsync(int pageSize)
        {
            PageSize = pageSize;
            await UpdateAsync().ConfigureAwait(false);
        }

        public void Refresh()
        {
            InvokeAsync(StateHasChanged);
        }

        public async Task UpdateAsync()
        {
            await LoadServerSideDataAsync().ConfigureAwait(false);
            Refresh();
        }

        private async Task LoadServerSideDataAsync()
        {
            if (DataLoader != null)
            {
                var filter = new Filter();

                var sortColumn = Columns.Find(x => x.SortColumn);
                if (sortColumn != null)
                    filter.Sorts.Add(new PropertySort
                    {
                        PropertyName = sortColumn.Field.GetPropertyMemberInfo()?.Name,
                        Desc = sortColumn.SortDescending
                    });

                filter.MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And
                };

                filter.MainNode.FilterNodes = Columns
                    .Where(c => c.FilterItem != null)
                    .Select(c => c.FilterItem)
                    .ToList();

                if (PageNumber > TotalPages)
                {
                    PageNumber = TotalPages - 1;
                }

                if (PageSize > 0)
                {
                    filter.Skip = PageNumber * PageSize;
                    filter.Take = PageSize;
                }

                var queryResult = await DataLoader.LoadDataAsync(filter).ConfigureAwait(false);
                Items = queryResult.items;
                TotalCount = queryResult.totalCount;
            }
        }

        public async Task FirstPageAsync()
        {
            if (PageNumber != 0)
            {
                PageNumber = 0;
                await UpdateAsync().ConfigureAwait(false);
            }
        }

        public async Task NextPageAsync()
        {
            if (PageNumber + 1 < TotalPages)
            {
                PageNumber++;
                await UpdateAsync().ConfigureAwait(false);
            }
        }

        public async Task PreviousPageAsync()
        {
            if (PageNumber > 0)
            {
                PageNumber--;
                await UpdateAsync().ConfigureAwait(false);
            }
        }

        public async Task LastPageAsync()
        {
            PageNumber = TotalPages - 1;
            await UpdateAsync().ConfigureAwait(false);
        }
    }
}
