using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using UnitTest.Utilities;

namespace UnitTest
{
	public class IntegrationTestSupplier
	{
		[Fact]
		public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
		{

			var options = ConnectionOptionHelper.Sqlite();
			// Arrange
			var supplier = new Supplier
			{
				Name = "Shoes Supplier",
				Description = "This is a Shoes Supplier",
				IsActive = true
			};
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var sut = new SupplierRepository(context);

				// Act
				sut.Create(supplier);
			}
			using (var context = new ECommerceDbContext(options))
			{
				// Assert
				var actual = context.Suppliers.Find(supplier.ID);

				Assert.NotNull(actual);
				Assert.Equal(supplier.Name, actual.Name);
				Assert.Equal(supplier.Description, actual.Description);

			}
		}
		[Fact]
		public void Retrieve_WithVAlidEntityID_ReturnsAValidEntity()
		{

			var options = ConnectionOptionHelper.Sqlite();
			// Arrange
			var supplier = new Supplier
			{
				Name = "Bag Supplier",
				Description = "This is a Bag Supplier"
			};
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				context.Suppliers.Add(supplier);
				context.SaveChanges();

			}
			using (var context = new ECommerceDbContext(options))
			{
				var sut = new SupplierRepository(context);
				// Act
				var actual = sut.Retrieve(supplier.ID);
				//Assert
				Assert.NotNull(actual);
				Assert.Equal(supplier.Name, actual.Name);
				Assert.Equal(supplier.Description, actual.Description);

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
				var sut = new SupplierRepository(context);

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
					context.Suppliers.Add(new Supplier
					{
						Name = string.Format("Supplier {0}", i),
						Description = string.Format("Description {0}", i),
						IsActive = true
					});
				}
				context.SaveChanges();
			}

			using (var context = new ECommerceDbContext(options))
			{
				var sut = new SupplierRepository(context);

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
			var supplier = new Supplier
			{
				Name = "Shirt Supplier",
				Description = "This is a Shirt Supplier",
				IsActive = true
			};
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				var sut = new SupplierRepository(context);
				context.Suppliers.Add(supplier);
				context.SaveChanges();
				//Act
				sut.Delete(supplier.ID);
				context.SaveChanges();
				var actual = context.Suppliers.Find(supplier.ID);
				//Assert
				Assert.Null(actual);
			}
			
		}
		[Fact]
		public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
		{
			var options = ConnectionOptionHelper.Sqlite();
			var oldSupplier = new Supplier
			{
				Name = "Shirt Supplier",
				Description = "This is a Shirt Supplier",
				IsActive = true
			};
			
			using (var context = new ECommerceDbContext(options))
			{
				context.Database.OpenConnection();
				context.Database.EnsureCreated();
				//Arrange
				context.Suppliers.Add(oldSupplier);
				context.SaveChanges();
				
			}

			var newName = "Shoes Supplier";
			var newDescription = "This is a Shoes Supplier";
			using (var context = new ECommerceDbContext(options))
			{
				var sut = new SupplierRepository(context);
				//Act
				var record = context.Suppliers.Find(oldSupplier.ID);
				record.Name = newName;
				record.Description = newDescription;
				sut.Update(record.ID, record);
				var newSupplier = context.Suppliers.Find(record.ID);
				//Assert
				Assert.Equal(newSupplier.Name, newName);
				Assert.Equal(newSupplier.Description, newDescription);
			}
			

		}
	}
}
