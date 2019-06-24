using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
	public class ProductRepository
		: RepositoryBase<Product>,
		IRepository<Product>
	{
		public ProductRepository(
			ECommerceDbContext context)
			:base(context)
		{

		}
	}
}
