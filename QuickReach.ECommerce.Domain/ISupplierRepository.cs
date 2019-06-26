using System;
using System.Collections.Generic;
using System.Text;
using QuickReach.ECommerce.Domain.Models;
namespace QuickReach.ECommerce.Domain
{
	public interface ISupplierRepository : IRepository<Supplier>
	{
		IEnumerable<Supplier> Retrieve(string search = "", int skip = 0, int count = 0);
	}
}
