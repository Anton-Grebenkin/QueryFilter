using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryFilter.Models
{
    public class QueryResult<T> where T : class
    {
        public IEnumerable<T> Items { get; }
        public int TotalCount { get; }

        public QueryResult(IEnumerable<T> items, int totalCount) 
        {  
            Items = items; 
            TotalCount = totalCount; 
        }
    }
}
