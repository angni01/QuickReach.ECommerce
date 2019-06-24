using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
	public class CategoryRepository
		: RepositoryBase<Category>,
		IRepository<Category>
	{
		public CategoryRepository(
			ECommerceDbContext context)
			: base(context)
		{

		}
	}
}
