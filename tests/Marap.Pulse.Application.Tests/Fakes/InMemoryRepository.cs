using Marap.Pulse.Domain.Common;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Marap.Pulse.Application.Tests.Fakes;

public class InMemoryRepository<TEntity, TId> : IRepository<TEntity, TId>
  where TEntity : Entity<TId>
  where TId : struct
{
  private readonly ConcurrentDictionary<int, TEntity> _store = new();
  private int _next = 1;

  public Task AddAsync(TEntity entity, CancellationToken cancellation = default)
  {
    // If entity has default id, try to assign a new one using a static "From" method on TId or via constructor
    int idValueInt = _next++;
    var tIdType = typeof(TId);
    object? newId = null;

    // Try to create new TId via static From(int) method
    var fromMethod = tIdType.GetMethod("From", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(int) }, null);
    if (fromMethod != null)
    {
      newId = fromMethod.Invoke(null, new object[] { idValueInt });
    }
    else
    {
      // Try ctor(int)
      var ctor = tIdType.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null);
      if (ctor != null)
      {
        newId = ctor.Invoke(new object[] { idValueInt });
      }
    }

    if (newId != null)
    {
      // set protected Id property via setter MethodInfo to allow non-public setter
      var prop = typeof(TEntity).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      var setMethod = prop!.GetSetMethod(true);
      setMethod!.Invoke(entity, new[] { newId });
    }

    _store[idValueInt] = entity;

    // Assign IDs for nested entity collections (e.g., PurchaseOrder.Lines)
    foreach (var prop in typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      if (prop.PropertyType == typeof(string)) continue;
      if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)) continue;

      var col = prop.GetValue(entity) as System.Collections.IEnumerable;
      if (col == null) continue;

      foreach (var item in col)
      {
        if (item == null) continue;
        var itemType = item.GetType();
        // only handle items that inherit from Entity<>
        var idProp = itemType.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (idProp == null) continue;

        var idVal = idProp.GetValue(item);
        // if default/uninitialized, assign new id
        var idType = idProp.PropertyType;
        bool isDefault = false;
        try { isDefault = idVal == null || idVal.Equals(Activator.CreateInstance(idType)); } catch { isDefault = idVal == null; }
        if (!isDefault) continue;

        var newNestedIdInt = _next++;
        object? nestedIdObj = null;
        var fromMethodNested = idType.GetMethod("From", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(int) }, null);
        if (fromMethodNested != null)
          nestedIdObj = fromMethodNested.Invoke(null, new object[] { newNestedIdInt });
        else
        {
          var ctorNested = idType.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null);
          if (ctorNested != null)
            nestedIdObj = ctorNested.Invoke(new object[] { newNestedIdInt });
        }

        if (nestedIdObj != null)
        {
          var setNested = idProp.GetSetMethod(true);
          setNested!.Invoke(item, new[] { nestedIdObj });
        }
      }
    }
    return Task.CompletedTask;
  }

  public Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellation = default)
  {
    // extract underlying int value from TId
    var tIdType = typeof(TId);
    var valueProp = tIdType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    var intVal = (int)valueProp!.GetValue(id)!;
    _store.TryGetValue(intVal, out var e);
    if (e != null)
    {
      // ensure nested items have ids
      foreach (var prop in typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)) continue;
        var col = prop.GetValue(e) as System.Collections.IEnumerable;
        if (col == null) continue;
        foreach (var item in col)
        {
          if (item == null) continue;
          var itemType = item.GetType();
          var idProp = itemType.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
          if (idProp == null) continue;
          var idVal = idProp.GetValue(item);
          var idType = idProp.PropertyType;
          bool isDefault = false;
          try { isDefault = idVal == null || idVal.Equals(Activator.CreateInstance(idType)); } catch { isDefault = idVal == null; }
          if (!isDefault) continue;

          var newNestedIdInt = _next++;
          object? nestedIdObj = null;
          var fromMethodNested = idType.GetMethod("From", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(int) }, null);
          if (fromMethodNested != null)
            nestedIdObj = fromMethodNested.Invoke(null, new object[] { newNestedIdInt });
          else
          {
            var ctorNested = idType.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null);
            if (ctorNested != null)
              nestedIdObj = ctorNested.Invoke(new object[] { newNestedIdInt });
          }

          if (nestedIdObj != null)
          {
            var setNested = idProp.GetSetMethod(true);
            setNested!.Invoke(item, new[] { nestedIdObj });
          }
        }
      }
    }
    return Task.FromResult(e);
  }

  public Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellation = default)
  {
    return Task.FromResult((IReadOnlyList<TEntity>)_store.Values.ToList());
  }

  public void Remove(TEntity entity)
  {
    var tIdType = typeof(TId);
    var valueProp = tIdType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    var intVal = (int)valueProp!.GetValue(entity.Id)!;
    _store.TryRemove(intVal, out _);
  }

  public void Update(TEntity entity)
  {
    var tIdType = typeof(TId);
    var valueProp = tIdType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    var intVal = (int)valueProp!.GetValue(entity.Id)!;
    _store[intVal] = entity;
  }

  public Task<int> SaveChangesAsync(CancellationToken cancellation = default) => Task.FromResult(0);
}
