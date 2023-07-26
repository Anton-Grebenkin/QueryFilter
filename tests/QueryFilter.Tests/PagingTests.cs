using QueryFilter.Extensions;
using QueryFilter.Models;
using QueryFilter.Tests.Extensions;
using System.Linq;

namespace QueryFilter.Tests
{
    public class PagingTests
    {
        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithSkipAndTake_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var filter = new Filter
            {
               Skip = 10,
               Take = 20,
            };

            //Act
            var expected = items.AsQueryable().Skip(filter.Skip.Value).Take(filter.Take.Value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
