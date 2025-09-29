# 📘 EF Core Mapping Guidelines (Pulse)

These guidelines define how we map our **domain entities** to the database using EF Core. They ensure mappings are **consistent, transparent, and maintainable**, while keeping the **domain model persistence‑agnostic**.

---

## 🎯 Principles

- **Domain purity**: Entities and value objects remain free of EF attributes.  
- **Per‑entity configuration**: Each entity has its own `IEntityTypeConfiguration<TEntity>` in `Infrastructure/Persistence/Configurations`.  
- **Transparency**: One file per entity, predictable naming.  
- **Strong typing**: All IDs are strongly‑typed (Vogen).  
- **Explicitness**: Value objects and enums are mapped deliberately.  
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

### Value Objects

- Mapped as **owned types**.  
- Always map the primitive property explicitly (`q.Property(x => x.Value).HasColumnName("...")`).  

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

    builder.OwnsOne(po => po.Status, status =>
    {
      status.Property(s => s.Value)
        .HasColumnName("Status")
        .IsRequired();
    });

    builder.Property(po => po.VendorId)
      .HasConversion(id => id.Value, value => VendorId.From(value));

    builder.HasOne<Vendor>().WithMany().HasForeignKey(po => po.VendorId);
  }
}
```

---

## 🚦 Adding a New Entity

1. Create the entity in `Domain/Entities` with a strongly‑typed ID.  
2. Add a config class in `Infrastructure/Persistence/Configurations`.  
3. Inherit from `EntityConfiguration<TEntity, TId, TPrimitive>`.  
4. Map scalar properties, owned value objects, relationships, and ignores.  
5. For nullable FKs, use a `ValueConverter<TId?,TPrimitive?>`.  
6. Run `dotnet ef migrations add <Name>` to validate mapping.  
7. Update diagrams + docs.  

---

## ✅ Benefits

- **Consistency**: Every entity follows the same pattern.  
- **Clarity**: Easy for new developers to find and understand mappings.  
- **Extensibility**: Adding new entities or value objects is straightforward.  
- **Safety**: Strong typing enforced at both domain and persistence layers.  
