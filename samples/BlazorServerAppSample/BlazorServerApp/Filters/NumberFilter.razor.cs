using BlazorServerApp.Components;
using BlazorServerApp.Interfaces;
using Microsoft.AspNetCore.Components;
using QueryFilter.Models;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace BlazorServerApp.Filters
{
    public partial class NumberFilter<TableItem> : IFilter<TableItem>
    {
        [CascadingParameter(Name = "Column")]
        public Column<TableItem> Column { get; set; }

        private OperatorType Condition { get; set; }

        private string FilterValue { get; set; }

        protected override void OnInitialized()
        {
            if (Column.Type.IsNumeric())
            {
                Column.FilterControl = this;

                if (Column.FilterItem != null)
                {
                    Condition = Column.FilterItem.Operator;
                    FilterValue = Column.FilterItem.Value.ToString();
                }
            }
        }

        public FilterItem GetFilter()
        {
            return new FilterItem
            {
                Operator = Condition,
                PropertyName = Column.Field.GetPropertyMemberInfo().Name,
                Value = FilterValue
            };
        }
    }

}
