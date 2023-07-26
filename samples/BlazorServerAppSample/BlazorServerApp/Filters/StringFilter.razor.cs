using BlazorServerApp.Components;
using BlazorServerApp.Interfaces;
using Microsoft.AspNetCore.Components;
using QueryFilter.Models;
using System;
using System.Linq.Expressions;

namespace BlazorServerApp.Filters
{
    public partial class StringFilter<TableItem> : IFilter<TableItem>
    {
        [CascadingParameter(Name = "Column")]
        public Column<TableItem> Column { get; set; }

        private OperatorType Condition { get; set; }

        private string  FilterText { get; set; }

        public Type FilterType => typeof(string);

        protected override void OnInitialized()
        {
            if (Column.Type == FilterType)
            {
                Column.FilterControl = this;

                if (Column.FilterItem != null)
                {
                    Condition = Column.FilterItem.Operator;
                    FilterText = Column.FilterItem.Value.ToString();
                }
            }
        }


        public FilterItem GetFilter()
        {
            FilterText = FilterText?.Trim();

            return new FilterItem
            {
                Operator = Condition,
                PropertyName = Column.Field.GetPropertyMemberInfo().Name,
                Value = FilterText
            };
        }

    }
}
