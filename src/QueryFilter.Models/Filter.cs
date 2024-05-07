namespace QueryFilter.Models
{
    public class Filter
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public FilterNode MainNode { get; set; }
        public ICollection<PropertySort> Sorts { get; set; } = new List<PropertySort>();
    }

    public class FilterNode
    {
        public LogicalOperatorType? LogicalOperator { get; set; }
        public string? PropertyName { get; set; }
        public ExpressionOperatorType? ExpressionOperator { get; set; }
        public object? Value { get; set; }
        public IEnumerable<FilterNode>? FilterNodes { get; set; }

    }

    public class PropertySort
    {
        public bool Desc { get; set; }
        public string PropertyName { get; set; }
    }
}
