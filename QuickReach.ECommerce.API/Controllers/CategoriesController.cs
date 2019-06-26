using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReachECommerce.Infra.Data;

namespace QuickReach.ECommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController: ControllerBase
	{
		private readonly ICategoryRepository repository;
		public CategoriesController(ICategoryRepository repository)
		{ 
			this.repository = repository;
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
	}
}
