# 📘 EF Core Migrations Guidelines (Pulse)

This document defines how we manage **Entity Framework Core migrations** in Pulse. It ensures migrations are **predictable, traceable, and safe** across environments.

---

## 🎯 Principles

- **Consistency**: All migrations follow the same naming and folder structure.  
- **Traceability**: Migration names reflect the business change, not just the technical detail.  
- **Safety**: Always validate migrations before applying to shared environments.  
- **Documentation**: Each migration is part of the project history and should be understandable at a glance.  

---

## 🏷 Naming Standards

- **Format**: `YYYYMMDD_HHMM_<ShortDescription>`  
  - Example: `20250928_0810_AddVendorToPurchaseOrder`  
- **Initial migration**: Always named `InitialCreate`.  
- **ShortDescription**:  
  - Use PascalCase.  
  - Describe the business/domain change (e.g., `AddStockItemLocation`, `RenameVendorLeadTime`).  
  - Avoid vague names like `FixStuff` or `UpdateSchema`.  

---

## 🏗 Commands

Run all commands from the **solution root** for consistency.

### Create a migration

```powershell
dotnet ef migrations add <Name> `
  --project src/Marap.Pulse.Infrastructure `
  --startup-project src/Marap.Pulse.Bootstrap `
  --output-dir Persistence/Migrations
```

Example:

```powershell
dotnet ef migrations add 20250928_0810_AddVendorToPurchaseOrder `
  --project src/Marap.Pulse.Infrastructure `
  --startup-project src/Marap.Pulse.Bootstrap `
  --output-dir Persistence/Migrations
```

### Apply migrations to database

```powershell
dotnet ef database update `
  --project src/Marap.Pulse.Infrastructure `
  --startup-project src/Marap.Pulse.Bootstrap
```

### Remove last migration (before applying)

```powershell
dotnet ef migrations remove `
  --project src/Marap.Pulse.Infrastructure `
  --startup-project src/Marap.Pulse.Bootstrap
```

---

## 🧪 Testing Migrations

1. **Build check**  
   Ensure solution builds cleanly before adding a migration.

2. **Scaffold migration**  
   Run `dotnet ef migrations add ...` and inspect the generated `.cs` files.  
   - Verify table/column names match domain intent.  
   - Ensure FKs and constraints are correct.  
   - Confirm no unintended drops/renames.

3. **Apply locally**  
   Run `dotnet ef database update` against your local dev DB.  
   - Check schema in SQL Server Management Studio / Azure Data Studio.  
   - Run smoke tests (basic inserts/queries).

4. **Rollback test (optional)**  
   Run `dotnet ef database update <PreviousMigration>` to ensure rollback works.

5. **Commit**  
   - Commit both the migration `.cs` files and the updated `ModelSnapshot`.  
   - Include migration name in commit message:  
     ```
     feat(migrations): 20250928_0810_AddVendorToPurchaseOrder
     ```

---

## 🚦 Best Practices

- **One migration per logical change**: Don’t batch unrelated changes.  
- **Review generated SQL**: Use `dotnet ef migrations script` to preview.  
- **Never edit applied migrations**: If a migration is already applied to shared DBs, create a new one for fixes.  
- **Keep Infrastructure clean**: All migrations live under `Infrastructure/Persistence/Migrations`.  
- **Sync with docs**: Update ER diagrams and EF Mapping Guidelines when schema changes.  

---

## ✅ Quick Checklist

- [ ] Build passes before migration.  
- [ ] Migration name follows `YYYYMMDD_HHMM_Description`.  
- [ ] Migration reviewed for correctness.  
- [ ] Applied locally and tested.  
- [ ] Committed with snapshot.  
- [ ] Documentation updated.  
