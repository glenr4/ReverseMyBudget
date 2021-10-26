using AutoFixture;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReverseMyBudget.Persistence.Sql.Tests
{
    public class IQueryableExtensionsTests
    {
        private Fixture _fixture;

        public IQueryableExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void WhenCreatePredicateWithString_ThenReturnsWhereContainsString()
        {
            // Arrange
            var paramValue = _fixture.Create<string>();
            var list = _fixture.CreateMany<Parameter>().AsQueryable();

            // Act
            var result = list.CreatePredicate(new Parameter { StringParam = paramValue }).ToString();

            // Assert
            result.Should().Contain("Where");
            result.Should().Contain($"StringParam.Contains(\"{paramValue}\"");
        }

        [Fact]
        public void WhenCreatePredicateWithGuid_ThenReturnsWhereEqualGuid()
        {
            // Arrange
            var paramValue = _fixture.Create<Guid>();
            var list = _fixture.CreateMany<Parameter>().AsQueryable();

            // Act
            var result = list.CreatePredicate(new Parameter { GuidParam = paramValue }).ToString();

            // Assert
            result.Should().Contain("Where");
            result.Should().Contain($"GuidParam == {paramValue}");
        }

        [Fact]
        public void GivenGuidNotNull_WhenCreatePredicateWithNullableGuid_ThenReturnsWhereEqualGuid()
        {
            // Arrange
            var paramValue = _fixture.Create<Guid?>();
            var list = _fixture.CreateMany<Parameter>().AsQueryable();

            // Act
            var result = list.CreatePredicate(new Parameter { NullableGuidParam = paramValue }).ToString();

            // Assert
            result.Should().Contain("Where");
            result.Should().Contain($"NullableGuidParam == Convert({paramValue}, Nullable`1)");
        }

        [Fact]
        public void GivenGuidIsNull_WhenCreatePredicateWithNullableGuid_ThenReturnsWhereEqualNull()
        {
            // Arrange
            Guid? paramValue = null;
            var list = _fixture.CreateMany<Parameter>().AsQueryable();

            // Act
            var result = list.CreatePredicate(new Parameter { NullableGuidParam = paramValue }).ToString();

            // Assert
            result.Should().Contain("Where");
            result.Should().Contain($"NullableGuidParam == null");
        }
    }

    public class Parameter
    {
        public string StringParam { get; set; }
        public Guid GuidParam { get; set; }
        public Guid? NullableGuidParam { get; set; }
    }
}