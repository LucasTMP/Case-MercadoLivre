using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public abstract class Repository<TEntity> : IDisposable where TEntity : Base
    {
        protected readonly ApiDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(ApiDbContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(predicate).ToListAsync();
        }

        public async Task<bool> EmailDuplicado(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(predicate).AnyAsync();
        }

        public async Task<bool> Existe(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(predicate).AnyAsync();
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }


        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
