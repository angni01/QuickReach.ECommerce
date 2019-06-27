using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
	public class CategoryRepository
		: RepositoryBase<Category>,
		ICategoryRepository
	{
		public CategoryRepository(
			ECommerceDbContext context)
			: base(context)
		{

		}

		public IEnumerable<Category> Retrieve(string search = "", int skip = 0, int count = 0)
		{
			var result = this.context.Set<Category>()
					.AsNoTracking()
					.Where(c => c.Name.Contains(search) ||
								c.Description.Contains(search))
					.Skip(skip)
					.Take(count)
					.ToList();
			return result;
		}

		public override Category Retrieve(int entityId)
		{
			var entity = this.context.Categories
						.Include(c => c.ProductCategories)
						.Include(c => c.ChildCategories)
						.Include(c => c.ParentCategories)
						.Where(c => c.ID == entityId)
						.FirstOrDefault();
			return entity;
		}

		//public override void Delete(int entityId)
		//{
		//	var products = this.context.Products.Where(c => c.CategoryID == entityId);
		//	if (products != null)
		//	{
		//		throw new SystemException("Cannot Delete Record");
		//	}
		//	var category = this.context.Set<Category>().Find(entityId);
		//	this.context.Set<Category>().Remove(category);
		//	this.context.SaveChanges();
		//}

	}
}
