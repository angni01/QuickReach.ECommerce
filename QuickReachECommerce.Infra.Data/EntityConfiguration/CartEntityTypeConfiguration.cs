using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
	class CartEntityTypeConfiguration
		: IEntityTypeConfiguration<Cart>
	{
		public void Configure(EntityTypeBuilder<Cart> builder)
		{
			builder.ToTable("Cart");
			builder.Property(p => p.ID)
				   .IsRequired()
				   .ValueGeneratedOnAdd();
			builder.HasMany(p => p.Items);
		}
	}
}
