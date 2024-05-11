# QueryFilter
A library that provides to apply serialized filters to IQueryable collections

## Features
- Filtering
- Paging
- Sorting

## Usage
Creating single filter with sorting and paging
```csharp
var filter = new Filter
{
   MainNode = new FilterNode
   {
      ExpressionOperator = ExpressionOperatorType.GreaterThan,
      PropertyName = nameof(Item.Id),
      Value = 1
   },
   Sorts = new List<PropertySort>()
   {
      new PropertySort
      {
         Desc = true,
         PropertyName = nameof(Item.Id)
      }
   },
   Skip = 10,
   Take = 100 
};

```

Creating multiple filters with a logical operator "or". It is possible to create many filters with nesting and logical operators and and or
```csharp
var filter = new Filter
{
   MainNode = new FilterNode
   {
      LogicalOperator = LogicalOperatorType.Or,
      FilterNodes = new List<FilterNode>
      {
         new FilterNode
         {
            ExpressionOperator = ExpressionOperatorType.GreaterThan,
            PropertyName = nameof(Item.Id),
            Value = 1
         },
         new FilterNode
         {
            ExpressionOperator = ExpressionOperatorType.LessThan,
            PropertyName = nameof(Item.Id),
            Value = 100
         }
      }
   },
   Sorts = new List<PropertySort>()
   {
      new PropertySort
      {
         Desc = true,
         PropertyName = nameof(Item.Id)
      }
   },
   Skip = 10,
   Take = 100 
};

```


Apply filter
```csharp
  public ActionResult<IEnumerable<Item>> GetItems([FromBody] Filter filter)
  {
    var items = db.Items.ApplyFilter(filter).ToList();
    return Ok(items);
  }
```
