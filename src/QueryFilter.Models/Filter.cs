namespace QueryFilter.Models
{
    public class Filter
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public ICollection<FilterItem> Items { get; set; } = new List<FilterItem>();
        public ICollection<PropertySort> Sorts { get; set; } = new List<PropertySort>();
    }

    public class FilterItem
    {
        public string PropertyName { get; set; }
        public OperatorType Operator { get; set; }
        public object? Value { get; set; }

    }

    public class PropertySort
    {
        public bool Desc { get; set; }
        public string PropertyName { get; set; }
    }

}
