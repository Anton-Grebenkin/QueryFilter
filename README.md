# QueryFilter
A library that provides to apply serialized filters to IQueryable collections

## Features
- Filtering
- Paging
- Sorting

## Usage
Create filter
```csharp
var filter = new Filter
{
   MainNode = new FilterNode
   {
      LogicalOperator = LogicalOperatorType.And,
      FilterNodes = new List<FilterNode>
      {
         new FilterNode
         {
            ExpressionOperator = ExpressionOperatorType.GreaterThan,
            PropertyName = nameof(Item.Id),
            Value = 1
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
