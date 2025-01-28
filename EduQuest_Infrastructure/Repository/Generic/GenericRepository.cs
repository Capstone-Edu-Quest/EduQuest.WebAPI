using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EduQuest_Infrastructure.Repository.Generic
{
	public class GenericRepository<TDomain> : IGenericRepository<TDomain> where TDomain : BaseEntity
	{
		private readonly ApplicationDbContext _context;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		//dbcontext can be seen as a unit of work pattern
		public IUnitOfWork UnitOfWork => _context;

		public async Task Add(TDomain entity)
		{
			
			entity.CreatedAt = DateTime.Now;
			entity.UpdatedAt = DateTime.Now;
			await _context.Set<TDomain>().AddAsync(entity);
			
		}

		public async Task<int> CountAsync()
		{
			return await _context.Set<TDomain>().AsNoTracking().CountAsync();
		}

		public async Task CreateRangeAsync(IEnumerable<TDomain> entities)
		{
			foreach (var entity in entities)
			{
				
				entity.CreatedAt = DateTime.Now;
				entity.UpdatedAt = DateTime.Now;
			}
			await _context.AddRangeAsync(entities);
		}

		public async Task Delete(dynamic id)
		{
			var entity = await _context.Set<TDomain>().FindAsync(id);
			if (entity == null)
			{
				return;
			}
			_context.Set<TDomain>().Remove(entity);
		}

		public async Task Delete(params dynamic[] id)
		{
			var entity = await _context.Set<TDomain>().FindAsync(id);
			if (entity == null)
			{
				return;
			}

			_context.Set<TDomain>().Remove(entity);
		}

		public async Task<TDomain> DeleteSoftAsync(string id)
		{
			TDomain _entity = await GetById(id);
			if (_entity == null)
			{
				return null;
			}
			
			_entity.DeletedAt = DateTime.Now;
			await Update(_entity);
			return _entity;
		}

		public async Task<IEnumerable<TDomain>> GetAll()
		{
			return await _context.Set<TDomain>().ToListAsync();
		}

		public async Task<PagedList<TDomain>> GetAll(int page, int eachPage)
		{
			var list = await _context.Set<TDomain>().ToListAsync();
			var totalItems = list.Count;
			var items = list.Skip((page - 1) * eachPage).Take(eachPage);

			return new PagedList<TDomain>(items, totalItems, page, eachPage);
		}

		public async Task<PagedList<TDomain>> GetAll(Expression<Func<TDomain, bool>> predicate, int page, int eachPage)
		{
			var list = await _context.Set<TDomain>().Where(predicate).ToListAsync();
			var totalItems = list.Count;
			var items = list.Skip((page - 1) * eachPage).Take(eachPage);

			return new PagedList<TDomain>(items, totalItems, page, eachPage);
		}



		public async Task<IEnumerable<TDomain>> GetAll(Expression<Func<TDomain, bool>> predicate)
		{
			return await _context.Set<TDomain>().Where(predicate).ToListAsync();
		}


		public async Task<PagedList<TDomain>> GetAll(int page, int eachPage, string sortBy, bool isAscending = false)
		{
			var entities = await _context.Set<TDomain>().PaginateAndSort(page, eachPage, sortBy, isAscending).ToListAsync();

			return new PagedList<TDomain>(entities, entities.Count, page, eachPage);

		}

		public async Task<PagedList<TDomain>> GetAll(Expression<Func<TDomain, bool>> predicate, int page, int eachPage, string sortBy, bool isAscending = true)
		{
			var entities = await _context.Set<TDomain>()
				.Where(predicate)
				.PaginateAndSort(page, eachPage, sortBy, isAscending).ToListAsync();

			return new PagedList<TDomain>(entities, entities.Count, page, eachPage);

		}

		public async Task<TDomain?> GetById(dynamic id)
		{
			return await _context.Set<TDomain>().FindAsync(id);
		}

		public Task Update(TDomain entity)
		{
			
			entity.UpdatedAt = DateTime.Now;
			var entry = _context.Entry(entity);
			if (entry.State == EntityState.Detached)
			{
				_context.Set<TDomain>().Attach(entity);
				entry.State = EntityState.Modified;
			}

			return Task.CompletedTask;
		}

		public async Task UpdateRangeAsync(IEnumerable<TDomain> entities)
		{
			_context.UpdateRange(entities);
		}
	}
}
