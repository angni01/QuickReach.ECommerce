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
	public class IntegrationTestForProduct
	{
		[Fact]
		public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
		{
			var options = ConnectionOptionHelper.Sqlite();
			//Arrange

			Product product;
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var category = new Category
				{
					Name = "Bag",
					Description = "Bag Department"
				};
				context.Categories.Add(category);
				context.SaveChanges();
				product = new Product
				{
					Name = "Sling Bag",
					Description = "This is a Sling Bag",
					Price = 199,
					//CategoryID = category.ID,
					ImageUrl = "slingbag.jpg"
				};
				
				
			}
			using (var context = new ECommerceDbContext(options))
			{
				var sut = new ProductRepository(context);

				//Act
				sut.Create(product);
				Assert.True(product.ID != 0);
				var actual = context.Products.Find(product.ID);
				//Assert
				Assert.NotNull(actual);
			}
				
		}
		[Fact]
		public void Create_WithNotExistingCategoryID_ShouldThrowException()
		{
			var options = ConnectionOptionHelper.Sqlite();
			//Arrange

			Product product;
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				product = new Product
				{
					Name = "Sling Bag",
					Description = "This is a Sling Bag",
					Price = 199,
					//CategoryID = 100,
					ImageUrl = "slingbag.jpg"
				};

				var sut = new ProductRepository(context);
				
				//Act &Assert
				Assert.Throws<DbUpdateException>(() => sut.Create(product));
			}

		}
		[Fact]
		public void Retrieve_WithVAlidEntityID_ReturnsAValidEntity()
		{

			var options = ConnectionOptionHelper.Sqlite();
			//Arrange
			Product product;

			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var category = new Category
				{
					Name = "Bag",
					Description = "Bag Department"
				};
				var categoryRepo = new CategoryRepository(context);
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
				var sut = new ProductRepository(context);
				//Act
				var retrievedProduct = sut.Retrieve(product.ID);
				//Assert
				Assert.NotNull(retrievedProduct);
				Assert.Equal(retrievedProduct.Name, product.Name);
				Assert.Equal(retrievedProduct.Description, product.Description);
				Assert.Equal(retrievedProduct.Price, product.Price);
				//Assert.Equal(retrievedProduct.CategoryID, product.CategoryID);
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
				//Arrange
				var sut = new ProductRepository(context);
				//Act
				var actual = sut.Retrieve(-1);
				//Assert
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
				var category = new Category
				{
					Name = "Bag",
					Description = "Bag Department"
				};
				context.Categories.Add(category);
				// Arrange
				for (int i = 1; i <= 25; i++)
				{
					context.Products.Add(new Product
					{
						Name = string.Format("Product {0}", i),
						Description = string.Format("Description {0}", i),
						Price = i,
						//CategoryID = category.ID,
						ImageUrl = string.Format("image{0}.jpg", i)
					});
				}
				context.SaveChanges();
			}
			using (var context = new ECommerceDbContext(options))
			{
				var sut = new ProductRepository(context);

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
			Product product;
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();

				var category = new Category
				{
					Name = "Bag",
					Description = "Bag Department"
				};
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
				//Act
				var sut = new ProductRepository(context);
				sut.Delete(product.ID);
				context.SaveChanges();
				var actual = context.Products.Find(product.ID);
				//Assert
				Assert.Null(actual);
			}
		}

		[Fact]
		public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
		{
			var options = ConnectionOptionHelper.Sqlite();
			//Arrange
			Product oldProduct;
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				//Arrange
				var category = new Category
				{
					Name = "Bag",
					Description = "Bag Department"
				};
				context.Categories.Add(category);
				oldProduct = new Product
				{
					Name = "Sling Bag",
					Description = "This is a Sling Bag",
					Price = 199,
					//CategoryID = category.ID,
					ImageUrl = "slingbag.jpg"
				};
				context.Products.Add(oldProduct);
				context.SaveChanges();
			}
			var newName = "Rubbes Shoes";
			var newDescription = "This is a Rubber Shoes";
			var newPrice = 500;
			var newImageUrl = "shoes.jpg";

			using (var context = new ECommerceDbContext(options))
			{
				var sut = new ProductRepository(context);
				var record = context.Products.Find(oldProduct.ID);
				//Act
				record.Name = newName;
				record.Description = newDescription;
				record.Price = newPrice;
				record.ImageUrl = newImageUrl;
				sut.Update(record.ID, record);
				var newProduct = context.Products.Find(record.ID);
				//Assert
				Assert.Equal(newName, newProduct.Name);
				Assert.Equal(newDescription, newProduct.Description);
				Assert.Equal(newPrice, newProduct.Price);
				Assert.Equal(newImageUrl, newProduct.ImageUrl);

			}

				
		}
	}
}
