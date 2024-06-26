﻿using QueryFilter.Models;
using QueryFilter.Tests.Extensions;
using QueryFilter.Extensions;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace QueryFilter.Tests
{
    public class FilteringTests
    {
        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorEqual_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var value = items.GetRandom().A;

            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode 
                        {
                            ExpressionOperator = ExpressionOperatorType.Equal,
                            PropertyName = nameof(DataItem.A),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.A == value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorNotEqual_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var value = items.GetRandom().A;

            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode
                        {
                            ExpressionOperator = ExpressionOperatorType.NotEqual,
                            PropertyName = nameof(DataItem.A),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.A != value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorContains_Should_MatchExpected(List<DataItem> items)
        {
            //Arrange
            var value = "a";

            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode
                        {
                            ExpressionOperator = ExpressionOperatorType.Contains,
                            PropertyName = nameof(DataItem.B),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.B.Contains(value)).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorContains_Should_ThrowInvalidCastException_WhenPropertyTypeNotString(List<DataItem> items)
        {
            //Arrange
            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                        {
                            new FilterNode
                            {
                                ExpressionOperator = ExpressionOperatorType.Contains,
                                PropertyName = nameof(DataItem.A),
                                Value = items.GetRandom().B
                            }
                        }
                }
            };

            //Act
            var act = () => items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            act.Should().Throw<FormatException>();
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorGreaterThan_Should_MatchExpected(List<DataItem> items, int value)
        {
            //Arrange
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
                            PropertyName = nameof(DataItem.A),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.A > value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorGreaterThanOrEqual_Should_MatchExpected(List<DataItem> items, int value)
        {
            //Arrange
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
                            PropertyName = nameof(DataItem.A),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.A >= value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorIn_Should_MatchExpected(List<DataItem> items, int[] values)
        {
            //Arrange
            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode
                        {
                            ExpressionOperator = ExpressionOperatorType.In,
                            PropertyName = nameof(DataItem.A),
                            Value = JsonSerializer.Serialize(values)
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => values.Contains(x.A)).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorIsNull_Should_MatchExpected(List<DataItem> items, int[] values)
        {
            //Arrange
            items.First().B = null;

            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode
                        {
                            ExpressionOperator = ExpressionOperatorType.IsNull,
                            PropertyName = nameof(DataItem.B)
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.B == null).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorLessThan_Should_MatchExpected(List<DataItem> items, int value)
        {
            //Arrange
            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode
                        {
                            ExpressionOperator = ExpressionOperatorType.LessThan,
                            PropertyName = nameof(DataItem.A),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.A < value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test, AutoMoqData(100)]
        public async Task ApplyFilter_WithOperatorLessThanOrEqual_Should_MatchExpected(List<DataItem> items, int value)
        {
            //Arrange
            var filter = new Filter
            {
                MainNode = new FilterNode
                {
                    LogicalOperator = LogicalOperatorType.And,
                    FilterNodes = new List<FilterNode>
                    {
                        new FilterNode
                        {
                            ExpressionOperator = ExpressionOperatorType.LessThanOrEqual,
                            PropertyName = nameof(DataItem.A),
                            Value = value
                        }
                    }
                }
            };

            //Act
            var expected = items.AsQueryable().Where(x => x.A <= value).ToList();
            var result = items.AsQueryable().ApplyFilter(filter).ToList();

            //Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
