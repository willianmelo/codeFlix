using CodeFlix.Catalog.Domain.Exceptions;
using System;
using System.Xml.Linq;
using Xunit;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActiveStatus(bool isActive)
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(isActive, category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = 
                () => new DomainEntity.Category(name!, "Category Description");

            var exception = Assert.Throws<EntityValidationException>(() => action());
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action =
                () => new DomainEntity.Category("Category Name", null);

            var exception = Assert.Throws<EntityValidationException>(() => action());
            Assert.Equal("Description should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameHasLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("a")]
        [InlineData("ab")]
        public void InstantiateErrorWhenNameHasLessThen3Characters(string name)
        {
            Action action =
              () => new DomainEntity.Category(name, "Category Description");

            var exception = Assert.Throws<EntityValidationException>(() => action());
            Assert.Equal("Name should has at least 3 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameGreaterThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameGreaterThen3Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

            Action action =
                () => new DomainEntity.Category(invalidName, "Category Description");

            var exception = Assert.Throws<EntityValidationException>(() => action());
            Assert.Equal("Name should has less then 255 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionGreaterThen10000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionGreaterThen10000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray());

            Action action =
                () => new DomainEntity.Category("Category Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationException>(() => action());
            Assert.Equal("Description should has less then 10.000 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Activate();

            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name", Description = "New Description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }


        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name" };
            var currentDescription = category.Description;

            category.Update(newValues.Name);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }
    }
}