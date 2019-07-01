using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.API.ViewModel;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReachECommerce.Infra.Data;
using Dapper;

namespace QuickReach.ECommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController: ControllerBase
	{
		private readonly ICategoryRepository repository;
		private readonly IProductRepository productrepository;
		private readonly ECommerceDbContext context;
		public CategoriesController(ICategoryRepository repository, IProductRepository productrepository
			,ECommerceDbContext context)
		{ 
			this.repository = repository;
			this.productrepository = productrepository;
			this.context = context;
		}
		// GET api/categories
		[HttpGet]
		public ActionResult Get(string search = "",int skip = 0, int count =10)
		{
			var categories = repository.Retrieve(search,skip,count);

			return Ok(categories);
		}

		// GET api/categories/5
		[HttpGet("{id}")]
		public ActionResult Get(int id)
		{
			var category = this.repository.Retrieve(id);
			return Ok(category);
		}

		// POST api/categories
		[HttpPost]
		public IActionResult Post([FromBody] Category category)
		{
			if (!this.ModelState.IsValid)
			{
				return BadRequest();
			}

			this.repository.Create(category);

			return CreatedAtAction(nameof(this.Get), category);
		}
		// POST api/categories/id
		[HttpPost("{categoryId}/products")]
		public IActionResult PostCategoryProduct(int categoryId, [FromBody] ProductCategory entity)
		{
			var category = this.repository.Retrieve(categoryId);
			var product = productrepository.Retrieve(entity.ProductID);
			if (category == null)
			{
				return NotFound();
			}
			if (product == null)
			{
				return NotFound();
			}

			category.AddProduct(entity.ProductID);
			repository.Update(categoryId, category);
			return Ok(category);
		}
		// PUT api/categories/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			

			var entity = this.repository.Retrieve(id);
			if (entity == null)
			{
				return NotFound();
			}

			this.repository.Update(id, category);

			return Ok(category);
		}

		// DELETE api/categories/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			this.repository.Delete(id);
			return Ok();
		}

		[HttpPut("{id}/products/{productId}")]
		public IActionResult DeleteCategoryProduct(int id, int productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var category = repository.Retrieve(id);
			if (category == null)
			{
				return NotFound();
			}
			if (productrepository.Retrieve(productId) == null)
			{
				return NotFound();
			}
			category.RemoveProduct(productId);
			this.repository.Update(id, category);
			return Ok();
		}

		[HttpGet("{id}/products")]
		public IActionResult GetProductByCategory(int id)
		{
			var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
			var connection = new SqlConnection(connectionString);
			var query =
				@"SELECT pc.ProductID,
							pc.CategoryID,
							p.Name,
							p.Description,
							p.Price,
							p.ImageUrl FROM
							Product p INNER JOIN
							ProductCategory pc ON p.ID = pc.ProductID
							WHERE pc.CategoryID = @categoryId";
			var categories = connection
				.Query<SearchItemViewModel>(
				query, new { categoryId = id })
				.ToList();

			return Ok(categories);
		}
	}
}
