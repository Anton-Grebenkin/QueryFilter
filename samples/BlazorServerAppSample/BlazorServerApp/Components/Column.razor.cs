using BlazorServerApp.Interfaces;
using Microsoft.AspNetCore.Components;
using QueryFilter.Models;
using System.Linq.Expressions;

namespace BlazorServerApp.Components
{
    public partial class Column<TableItem>
    {
        [CascadingParameter(Name = "Table")]
        public Table<TableItem> Table { get; set; }

        [Parameter]
        public Expression<Func<TableItem, object>> Field { get; set; }
        
        [Parameter]
        public bool Filterable { get; set; }

        [Parameter]
        public bool Sortable { get; set; }

        private string _title;
        [Parameter]
        public string Title
        {
            get { return _title ?? Field.GetPropertyMemberInfo()?.Name; }
            set { _title = value; }
        }

        [Parameter]
        public Type Type { get; set; }

        public bool FilterOpen { get; private set; }

        public ElementReference FilterRef { get; set; }

        public IFilter<TableItem> FilterControl { get; set; }

        public FilterNode FilterItem { get; set; }


        public bool SortColumn { get; set; }
        public bool SortDescending { get; set; }

        public string Render(TableItem item)
        {
            if (item == null || Field == null) return string.Empty;

            if (renderCompiled == null)
                renderCompiled = Field.Compile();

            object value = null;

            try
            {
                value = renderCompiled.Invoke(item);
            }
            catch (NullReferenceException) { }

            if (value == null) return string.Empty;

            return value.ToString();     
        }

        public void ToggleFilter()
        {
            FilterOpen = !FilterOpen;
            Table.Refresh();
        }

        public async Task SortByAsync()
        {
            if (Sortable)
            {
                if (SortColumn)
                {
                    SortDescending = !SortDescending;
                }

                Table.Columns.ForEach(x => x.SortColumn = false);

                SortColumn = true;

                await Table.UpdateAsync().ConfigureAwait(false);
            }
        }

        protected override void OnInitialized()
        {
            Table.AddColumn(this);
        }

        private Func<TableItem, object> renderCompiled;
    }
}
