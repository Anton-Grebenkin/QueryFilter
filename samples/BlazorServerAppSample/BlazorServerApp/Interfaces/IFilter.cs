using QueryFilter.Models;

namespace BlazorServerApp.Interfaces
{
    public interface IFilter<TableItem>
    {
        FilterNode GetFilter();
    }
}
