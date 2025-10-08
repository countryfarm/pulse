namespace Marap.Pulse.Domain.Common;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IRepository<TEntity, TId>
  where TEntity : Entity<TId>
  where TId : struct
{
  Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellation = default);

  Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellation = default);

  Task AddAsync(TEntity entity, CancellationToken cancellation = default);

  void Update(TEntity entity);

  void Remove(TEntity entity);

  Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
