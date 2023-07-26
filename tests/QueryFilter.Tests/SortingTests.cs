using QueryFilter.Extensions;
using QueryFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryFilter.Tests
{
    public class SortingTests
    {
        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithSortingByDesc_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var filter = new Filter
            {
                Sorts = new List<PropertySort>
                {
                    new PropertySort
                    {
                        Desc = true,
                        PropertyName = nameof(DataItem.A),
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().OrderByDescending(i => i.A).ToArray();
            var result = items.AsQueryable().ApplyFilter(filter).ToArray();

            //Assert
            result.Count().Should().Be(expected.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                expected[i].Should().BeEquivalentTo(result[i]);
            }
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithSortingByAsc_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var filter = new Filter
            {
                Sorts = new List<PropertySort>
                {
                    new PropertySort
                    {
                        Desc = false,
                        PropertyName = nameof(DataItem.A),
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().OrderBy(i => i.A).ToArray();
            var result = items.AsQueryable().ApplyFilter(filter).ToArray();

            //Assert
            result.Count().Should().Be(expected.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                expected[i].Should().BeEquivalentTo(result[i]);
            }
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithManySorts_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var filter = new Filter
            {
                Sorts = new List<PropertySort>
                {
                    new PropertySort
                    {
                        Desc = false,
                        PropertyName = nameof(DataItem.A),
                    },
                    new PropertySort
                    {
                        Desc = true,
                        PropertyName = nameof(DataItem.B),
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().OrderBy(i => i.A).ThenByDescending(i => i.B).ToArray();
            var result = items.AsQueryable().ApplyFilter(filter).ToArray();

            //Assert
            result.Count().Should().Be(expected.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                expected[i].Should().BeEquivalentTo(result[i]);
            }
        }
    }
}
