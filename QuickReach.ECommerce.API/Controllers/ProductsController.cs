using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuickReach.ECommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : Controller
	{
		private readonly IProductRepository repository;
		public ProductsController(IProductRepository repository)
		{
			this.repository = repository;
		}
		// GET: api/<controller>
		[HttpGet]
		public ActionResult Get(string search = "", int skip = 0, int count = 10)
		{
			var products = repository.Retrieve(search, skip, count);

			return Ok(products);
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public ActionResult Get(int id)
		{
			var product = this.repository.Retrieve(id);
			return Ok(product);
		}

		// POST api/<controller>
		[HttpPost]
		public IActionResult Post([FromBody] Product product)
		{
			if (!this.ModelState.IsValid)
			{
				return BadRequest();
			}

			this.repository.Create(product);

			return CreatedAtAction(nameof(this.Get), product);
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Product product)
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

			this.repository.Update(id, product);

			return Ok(product);
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			this.repository.Delete(id);
			return Ok();
		}
	}
}
