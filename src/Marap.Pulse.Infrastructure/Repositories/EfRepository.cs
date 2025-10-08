using Marap.Pulse.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Marap.Pulse.Infrastructure.Repositories;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId>
  where TEntity : Marap.Pulse.Domain.Common.Entity<TId>
  where TId : struct
{
  private readonly PulseDbContext _db;
  private readonly DbSet<TEntity> _set;

  public EfRepository(PulseDbContext db)
  {
    _db = db;
    _set = db.Set<TEntity>();
  }

  public async Task AddAsync(TEntity entity, CancellationToken cancellation = default)
  {
    await _set.AddAsync(entity, cancellation).ConfigureAwait(false);
  }

  public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellation = default)
  {
    return await _set.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);
  }

  public async Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellation = default)
  {
    return await _set.ToListAsync(cancellation).ConfigureAwait(false);
  }

  public void Remove(TEntity entity)
  {
    _set.Remove(entity);
  }

  public void Update(TEntity entity)
  {
    _set.Update(entity);
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
  {
    return await _db.SaveChangesAsync(cancellation).ConfigureAwait(false);
  }
}
