using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UnitTest.Utilities;

namespace UnitTest
{
	public class UnitTest1
	{
		[Fact]
		public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
		{

			var options = ConnectionOptionHelper.Sqlite();
			var category = new Category
			{
				Name = "Bag",
				Description = "Bag Department"
			};
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var sut = new CategoryRepository(context);
				//Act
				sut.Create(category);
			}

			using (var context = new ECommerceDbContext(options))
			{
				//Assert
				Assert.True(category.ID != 0);

				var entity = context.Categories.Find(category.ID);
				Assert.NotNull(entity);
			}

			
		}
		[Fact]
		public void Retrieve_WithVAlidEntityID_ReturnsAValidEntity()
		{
			var options = ConnectionOptionHelper.Sqlite();
			//Arrange

			var category = new Category
			{
				Name = "Shoes",
				Description = "Shoes Department"
			};
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				context.Categories.Add(category);
				context.SaveChanges();
			}

			using (var context = new ECommerceDbContext(options))
			{
				var sut = new CategoryRepository(context);
				// Act
				var actual = sut.Retrieve(category.ID);
				//Assert
				Assert.NotNull(actual);
				Assert.Equal(category.Name, actual.Name);
				Assert.Equal(category.Description, actual.Description);
			}
			
			
		}

		[Fact]
		public void Retrieve_WithNonexistingEntityID_ReturnsNull()
		{
			var options = ConnectionOptionHelper.Sqlite();

			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				// Arrange
				var sut = new CategoryRepository(context);

				// Act
				var actual = sut.Retrieve(-1);

				// Assert
				Assert.Null(actual);
			}
		}

		[Fact]

		public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
		{
			var options = ConnectionOptionHelper.Sqlite();
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				// Arrange
				for (int i = 1; i <= 25; i++)
				{
					context.Categories.Add(new Category
					{
						Name = string.Format("Category {0}", i),
						Description = string.Format("Description {0}", i)
					});
				}
				context.SaveChanges();
			}
			using (var context = new ECommerceDbContext(options))
			{
				var sut = new CategoryRepository(context);

				// Act & Assert
				var list = sut.Retrieve(5, 5);
				Assert.True(list.Count() == 5);

				list = sut.Retrieve(0, 5);
				Assert.True(list.Count() == 5);

				list = sut.Retrieve(10, 5);
				Assert.True(list.Count() == 5);

				list = sut.Retrieve(15, 5);
				Assert.True(list.Count() == 5);

				list = sut.Retrieve(20, 5);
				Assert.True(list.Count() == 5);
			}
		}

		[Fact]
		public void Delete_WithValidEntityID_ShouldRemoveRecordFromDatabase()
		{
			var options = ConnectionOptionHelper.Sqlite();
			//Arrange
			var category = new Category
			{
				Name = "Bag",
				Description = "Bag Department"
			};

			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var sut = new CategoryRepository(context);
				context.Categories.Add(category);
				context.SaveChanges();
				//Act
				sut.Delete(category.ID);
				context.SaveChanges();
				var actual = context.Categories.Find(category.ID);
				//Assert
				Assert.Null(actual);
			}
		}


		[Fact]
		public void Delete_WithExistingProduct_ShouldThrowAnException()
		{
			var options = ConnectionOptionHelper.Sqlite();
			//Arrange
			//Arrange
			var category = new Category
			{
				Name = "Bag",
				Description = "Bag Department"
			};
			Product product;
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var sut = new CategoryRepository(context);
				context.Categories.Add(category);
				
				product = new Product
				{
					Name = "Sling Bag",
					Description = "This is a Sling Bag",
					Price = 199,
					//CategoryID = category.ID,
					ImageUrl = "slingbag.jpg"
				};
				context.Products.Add(product);
				context.SaveChanges();
			}
			using (var context = new ECommerceDbContext(options))
			{
				var sut = new CategoryRepository(context);
				//Act &Assert
				Assert.Throws<DbUpdateException>(() => sut.Delete(product.ID));
			}
		}
		[Fact]
		public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
		{
			var options = ConnectionOptionHelper.Sqlite();
			var oldCategory = new Category
			{
				Name = "shoes",
				Description = "Shoes Department"
			};

			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				//Arrange
				context.Categories.Add(oldCategory);
				context.SaveChanges();

			}
			var newName = "Bag";
			var newDescription = "Bag Department";

			using (var context = new ECommerceDbContext(options))
			{
				var sut = new CategoryRepository(context);
				//Act
				var record = context.Categories.Find(oldCategory.ID);
				record.Name = newName;
				record.Description = newDescription;
				sut.Update(record.ID, record);
				var newCategory = context.Categories.Find(record.ID);
				//Assert
				Assert.Equal(newCategory.Name, newName);
				Assert.Equal(newCategory.Description, newDescription);
			}
			
		}

	}
}
