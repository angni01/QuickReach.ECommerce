using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
	
{
	public class ProductSupplierEntityTypeConfiguration
		: IEntityTypeConfiguration<ProductSupplier>
	{
		public void Configure(EntityTypeBuilder<ProductSupplier> builder)
		{
			builder.ToTable("ProductSupplier");
			builder.HasKey(ps => new { ps.SupplierID, ps.ProductID });
			builder.HasOne(ps => ps.Supplier)
				   .WithMany(c => c.ProductSuppliers)
				   .HasForeignKey("SupplierID");
			builder.HasOne(cr => cr.Product)
				   .WithMany(c => c.ProductSuppliers)
				   .HasForeignKey("ProductID");
		}
	}
}
