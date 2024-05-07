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

        private ExpressionOperatorType Condition { get; set; }

        private string FilterValue { get; set; }

        protected override void OnInitialized()
        {
            if (Column.Type.IsNumeric())
            {
                Column.FilterControl = this;

                if (Column.FilterItem != null)
                {
                    Condition = Column.FilterItem.ExpressionOperator.Value;
                    FilterValue = Column.FilterItem.Value.ToString();
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
