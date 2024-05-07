using BlazorServerApp.Components;
using BlazorServerApp.Interfaces;
using Microsoft.AspNetCore.Components;
using QueryFilter.Models;
using System;
using System.Linq.Expressions;

namespace BlazorServerApp.Filters
{
    public partial class DateFilter<TableItem> : IFilter<TableItem>
    {
        [CascadingParameter(Name = "Column")]
        public Column<TableItem> Column { get; set; }

        private ExpressionOperatorType Condition { get; set; }

        private DateTime FilterValue { get; set; } = DateTime.Now;

        protected override void OnInitialized()
        {
            if (Column.Type.GetNonNullableType() == typeof(DateTime))
            {
                Column.FilterControl = this;
   
                if (Column.FilterItem != null && DateTime.TryParse(Column.FilterItem.Value.ToString(), out DateTime result))
                {
                    Condition = Column.FilterItem.ExpressionOperator.Value;
                    FilterValue = result;
                }
            }
        }

        public FilterNode GetFilter()
        {
            return new FilterNode
            {
                ExpressionOperator = Condition,
                PropertyName = Column.Field.GetPropertyMemberInfo().Name,
                Value = FilterValue
            };
        }
    }
}