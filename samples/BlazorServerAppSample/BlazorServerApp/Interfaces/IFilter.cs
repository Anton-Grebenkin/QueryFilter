using QueryFilter.Models;

namespace BlazorServerApp.Interfaces
{
    public interface IFilter<TableItem>
    {
        FilterItem GetFilter();
    }
}
