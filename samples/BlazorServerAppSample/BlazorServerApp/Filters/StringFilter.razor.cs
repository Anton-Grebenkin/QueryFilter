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

        private ExpressionOperatorType Condition { get; set; }

        private string  FilterText { get; set; }

        public Type FilterType => typeof(string);

        protected override void OnInitialized()
        {
            if (Column.Type == FilterType)
            {
                Column.FilterControl = this;

                if (Column.FilterItem != null)
                {
                    Condition = Column.FilterItem.ExpressionOperator.Value;
                    FilterText = Column.FilterItem.Value.ToString();
                }
            }
        }


        public FilterNode GetFilter()
        {
            FilterText = FilterText?.Trim();

            return new FilterNode
            {
                ExpressionOperator = Condition,
                PropertyName = Column.Field.GetPropertyMemberInfo().Name,
                Value = FilterText
            };
        }

    }
}
