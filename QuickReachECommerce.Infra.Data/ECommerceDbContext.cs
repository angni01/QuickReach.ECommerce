﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickReach.ECommerce.Domain.Models;
using System;

namespace QuickReachECommerce.Infra.Data
{
	public class ECommerceDbContext
		: DbContext
	{
		public ECommerceDbContext(
			DbContextOptions<ECommerceDbContext> options)
			:base(options)
		{

		}
		public ECommerceDbContext() :base()
		{

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionString =
				"Server=.;Database=QuickReachDb;Integrated Security=true;";
			optionsBuilder.UseSqlServer(connectionString);
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

	}



}
