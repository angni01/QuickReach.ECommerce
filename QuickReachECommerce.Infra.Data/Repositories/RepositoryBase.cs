using QuickReach.ECommerce.Domain;
using QuickReachECommerce.Infra.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
	public abstract class RepositoryBase<TEntity>
		: IRepository<TEntity> where TEntity : EntityBase
	{
		protected readonly ECommerceDbContext context;
		public RepositoryBase(ECommerceDbContext context)
		{
			this.context = context;
		}

		public virtual TEntity Create(TEntity newEntity)
		{
			this.context.Set<TEntity>()
						.Add(newEntity);
			this.context.SaveChanges();
			return newEntity;
		}

		public virtual void Delete(int entityId)
		{
			var entityToRemove = Retrieve(entityId);
			this.context.Remove<TEntity>(entityToRemove);
			this.context.SaveChanges();
		}

		public virtual TEntity Retrieve(int entityId)
		{
			var entity = this.context
				.Set<TEntity>()
				.AsNoTracking()
				.FirstOrDefault(c => c.ID == entityId);
			return entity;
		}

		public IEnumerable<TEntity> Retrieve(int skip = 0, int count = 1)
		{
			var result = this.context.Set<TEntity>()
					.AsNoTracking()
					.Skip(skip)
					.Take(count)
					.ToList();
			return result;
		}
		public TEntity Update(int entityId, TEntity entity)
		{
			var entry = this.context.Update<TEntity>(entity);

			this.context.SaveChanges();

			return entry.Entity;
		}
	}
}
