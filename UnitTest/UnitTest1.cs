using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTest
{
	public class UnitTest1
	{
		[Fact]
		public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
		{
			var context = new ECommerceDbContext();
			var sut = new CategoryRepository(context);
			var category = new Category
			{
				Name = "Bag",
				Description = "Bag Department"
			};

			//Act
			sut.Create(category);

			//Assert
			Assert.True(category.ID != 0);

			var entity = sut.Retrieve(category.ID);
			Assert.NotNull(entity);

			//Cleanup
			//sut.Delete(category.ID);
		}
		[Fact]
		public void Retrieve_WithVAlidEntityID_ReturnsAValidEntity()
		{
			//Arrange
			var context = new ECommerceDbContext();
			var category = new Category
			{
				Name = "Shoes",
				Description = "Shoes Department"
			};
			var sut = new CategoryRepository(context);
			sut.Create(category);
			//Act
			var actual = sut.Retrieve(category.ID);

			//Assert
			Assert.NotNull(actual);

			//Cleanup
			sut.Delete(actual.ID);
		}

		[Fact]
		public void Retrieve_WithNonexistingEntityID_ReturnsNull()
		{
			//Arrange
			var context = new ECommerceDbContext();
			var sut = new CategoryRepository(context);
			//Act
			var actual = sut.Retrieve(-1);

			//Assert
			Assert.Null(actual);
		}

		[Fact]

		public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
		{
			//Arrange
			var context = new ECommerceDbContext();
			var sut = new CategoryRepository(context);
			for (var i = 1; i <= 20; i += 1)
			{
				sut.Create(new Category
				{
					Name = string.Format("Category {0}", i),
					Description = string.Format("Description {0}", i)
				});
			}
			//Act
			var list = sut.Retrieve(5, 5);
			//Assert
			Assert.True(list.Count() == 5);
			//Cleanup
			list = sut.Retrieve(0, 41);
			foreach (var item in list)
			{
				sut.Delete(item.ID);
			}
		}

		[Fact]
		public void Delete_WithValidEntityID_ShouldRemoveRecordFromDatabase()
		{
			//Arrange
			var context = new ECommerceDbContext();
			var sut = new CategoryRepository(context);
			var category = new Category
			{
				Name = "Bag",
				Description = "Bag Department"
			};
			sut.Create(category);
			var record = sut.Retrieve(category.ID);
			//Act
			sut.Delete(record.ID);
			var actual = sut.Retrieve(record.ID);
			//Assert
			Assert.Null(actual);
		}
		[Fact]
		public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
		{
			var context = new ECommerceDbContext();
			var sut = new CategoryRepository(context);
			var category = new Category
			{
				Name = "shoes",
				Description = "Shoes Department"
			};
			sut.Create(category);
			var record = sut.Retrieve(category.ID);
			var expectedName = "Bag";
			var expectedDescription = "Bag Department";

			//Act
			record.Name = expectedName;
			record.Description = expectedDescription;

			var actual = sut.Update(record.ID, category);
			//Assert
			Assert.Equal(expectedName, actual.Name);
			Assert.Equal(expectedDescription, actual.Description);

			//Cleanup
			sut.Delete(actual.ID);
		}

	}
}
