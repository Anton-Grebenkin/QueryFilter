using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BlazorServerApp.Components
{
    public partial class FilterManager<TableItem>
    {
        [CascadingParameter(Name = "Column")]
        public Column<TableItem> Column { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private async Task ApplyFilterAsync()
        {
            Column.ToggleFilter();

            if (Column.FilterControl != null)
            {
                Column.FilterItem = Column.FilterControl.GetFilter();
                await Column.Table.UpdateAsync().ConfigureAwait(false);
            }
        }
        private async Task ClearFilterAsync()
        {
            Column.ToggleFilter();

            if (Column.FilterItem != null)
            {
                Column.FilterItem = null;
                await Column.Table.UpdateAsync().ConfigureAwait(false);
            }
        }
    }
}
