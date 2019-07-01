using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
	public class ProductRepository
		: RepositoryBase<Product>,
		IProductRepository
	{
		public ProductRepository(
			ECommerceDbContext context)
			:base(context)
		{

		}

		//public override Product Create(Product product)
		//{
		//	var category = this.context.Categories.Find(product.ID);
		//	if (category == null)
		//	{
		//		throw new SystemException("Record does not Exist!");
		//	}

		//	this.context.Set<Product>().Add(product);
		//	this.context.SaveChanges();
		//	return product;
		//}

		public IEnumerable<Product> Retrieve(string search = "", int skip = 0, int count = 0)
		{
			var result = this.context.Products
				.Where(c => c.Name.Contains(search) ||
						   c.Description.Contains(search))
				.Skip(skip)
				.Take(count)
				.ToList();
			return result;
		}

		//public IEnumerable<Product> GetByCategory(int categoryId)
		//{
		//	var product = this.context.Products.AsNoTracking().Include(p => p.Product(categories).)

		//	return null;
		//}
	}
}
