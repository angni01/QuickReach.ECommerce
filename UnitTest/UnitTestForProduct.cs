using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace UnitTest
{
	public class UnitTestForProduct
	{
		[Fact]
		public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
		{
			//Arrange
			var context = new ECommerceDbContext();
			var sut = new ProductRepository(context);
			var product = new Product
			{
				Name = "Sling Bag",
				Description = "This is a Sling Bag",
				Price = 199,
				CategoryID = 51,
				ImageUrl = "slingbag.jpg"			   				 
			};
			//Act
			sut.Create(product);
			Assert.True(product.ID != 0);
			var actual = sut.Retrieve(product.ID);
			//Assert
			Assert.NotNull(actual);
			//Cleanup
			sut.Delete(product.ID);
		}
		[Fact]
		public void Retrieve_WithVAlidEntityID_ReturnsAValidEntity()
		{

		}
}
