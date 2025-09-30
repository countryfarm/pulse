# 📘 EF Core Mapping Guidelines (Pulse)

These guidelines define how we map our **domain entities** to the database using EF Core. They ensure mappings are **consistent, transparent, and maintainable**, while keeping the **domain model persistence‑agnostic**.

---

## 🎯 Principles

- **Domain purity**: Entities and value objects remain free of EF attributes.  
- **Per‑entity configuration**: Each entity has its own `IEntityTypeConfiguration<TEntity>` in `Infrastructure/Persistence/Configurations`.  
- **Transparency**: One file per entity, predictable naming.  
- **Strong typing**: All IDs and value objects are strongly‑typed (Vogen).  
- **Explicitness**: Value objects, enums, and conversions are mapped deliberately.  
- **Minimalism**: Only business‑meaningful properties appear in diagrams/docs.  

---

## 🏗 Structure

- **Base class**:  
  `EntityConfiguration<TEntity, TId, TPrimitive>` handles strongly‑typed IDs for primary keys.  

- **Entity configs**:  
  One per entity (`PartConfiguration`, `StockItemConfiguration`, etc.).  

- **DbContext**:  
  Uses `modelBuilder.ApplyConfigurationsFromAssembly(...)` to auto‑apply all configs.  

---

## 🔑 Mapping Rules

### IDs

- **Primary keys**:  
  Mapped via the base class with `.HasConversion(id => id.Value, value => TId.From(value))`.  
- **Foreign keys**:  
  - **Non‑nullable** IDs → simple conversion.  
  - **Nullable** IDs → use a `ValueConverter<TId?,TPrimitive?>` to handle nulls safely.  

### Value Objects (Vogen)

- **Do not use `OwnsOne`** for Vogen structs. Treat them as scalar properties.  
- Map with `.Property(e => e.VoProperty).HasVogenConversion(...)`.  
- Use the shared extension methods:  
  - **Non‑nullable**:  
    ```csharp
    builder.Property(e => e.MinimumThreshold)
      .HasVogenConversion(
        vo  => vo.Value,
        raw => Quantity.From(raw))
      .HasColumnName("MinimumThreshold")
      .IsRequired();
    ```
  - **Nullable**:  
    ```csharp
    builder.Property(e => e.ReceivedQuantity)
      .HasVogenConversion(
        vo  => vo.Value,
        raw => Quantity.From(raw))
      .HasColumnName("ReceivedQuantity")
      .IsRequired(false);
    ```
- This ensures EF Core stores the primitive (`decimal`, `string`, etc.) while the domain works with the VO.

### Enums

- Stored as **strings** for readability and forward compatibility.  

### Relationships

- Use `.HasOne().WithMany()` or `.HasOne().WithMany(nav => nav.Collection)` depending on navigation.  
- Always pair with FK property conversions.  

### Ignored Members

- Ignore domain plumbing (`Events`, computed properties like `TotalQuantity`).  

---

## ⚠️ Nullable Foreign Keys

For optional relationships (e.g. `StockItem.VendorId?`):

```csharp
builder.Property(s => s.VendorId)
  .HasConversion(new ValueConverter<VendorId?, int?>(
    id => id.HasValue ? id.Value.Value : (int?)null,
    value => value.HasValue ? VendorId.From(value.Value) : (VendorId?)null));
```

---

## 🧩 Example Pattern

```csharp
public class PurchaseOrderConfiguration 
  : EntityConfiguration<PurchaseOrder, PurchaseOrderId, int>
{
  public PurchaseOrderConfiguration() 
    : base(id => id.Value, value => PurchaseOrderId.From(value)) { }

  public override void Configure(EntityTypeBuilder<PurchaseOrder> builder)
  {
    base.Configure(builder);

    builder.Property(po => po.OrderDate).IsRequired();

    builder.Property(po => po.Status)
      .HasVogenConversion(
        vo  => vo.Value,
        raw => PurchaseOrderStatus.From(raw))
      .HasColumnName("Status")
      .IsRequired();

    builder.Property(po => po.VendorId)
      .HasConversion(id => id.Value, value => VendorId.From(value));

    builder.HasOne<Vendor>().WithMany().HasForeignKey(po => po.VendorId);
  }
}
```

---

## 🚦 Adding a New Entity

1. Create the entity in `Domain/Entities` with a strongly‑typed ID and any Vogen value objects.  
2. Add a config class in `Infrastructure/Persistence/Configurations`.  
3. Inherit from `EntityConfiguration<TEntity, TId, TPrimitive>`.  
4. Map scalar properties, value objects (with `HasVogenConversion`), relationships, and ignores.  
5. For nullable FKs, use a `ValueConverter<TId?,TPrimitive?>`.  
6. Run `dotnet ef migrations add <Name>` to validate mapping.  
7. Update diagrams + docs.  

---

## ✅ Benefits

- **Consistency**: Every entity follows the same pattern.  
- **Clarity**: Easy for new developers to find and understand mappings.  
- **Extensibility**: Adding new entities or value objects is straightforward.  
- **Safety**: Strong typing enforced at both domain and persistence layers.  
- **Correctness**: Vogen value objects are persisted as primitives, avoiding EF Core owned‑type pitfalls.  
