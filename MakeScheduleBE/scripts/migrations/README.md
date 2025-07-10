# Database Migration Scripts

This folder contains batch files to help manage database migrations for both development (SQLite) and production (SQL Server) environments.

## 📁 Folder Structure

```
scripts/
└── migrations/
    ├── create-sqlite-migration.bat
    ├── create-sqlserver-migration.bat
    ├── update-sqlite-database.bat
    ├── update-sqlserver-database.bat
    └── README.md
```

## 🚀 Available Scripts

### Creating Migrations

1. **`create-sqlite-migration.bat`** - Creates migrations for SQLite (Development)

   ```bash
   create-sqlite-migration.bat "MigrationName"
   ```

2. **`create-sqlserver-migration.bat`** - Creates migrations for SQL Server (Production)
   ```bash
   create-sqlserver-migration.bat "MigrationName"
   ```

### Applying Migrations

3. **`update-sqlite-database.bat`** - Applies migrations to SQLite database

   ```bash
   update-sqlite-database.bat
   ```

4. **`update-sqlserver-database.bat`** - Applies migrations to SQL Server database
   ```bash
   update-sqlserver-database.bat
   ```

### Fixing Migration Issues

5. **`fix-migration-history.bat`** - Fixes general migration history issues

   ```bash
   fix-migration-history.bat
   ```

6. **`fix-aspnet-tables-migration.bat`** - Specifically fixes AspNet tables migration issues

   ```bash
   fix-aspnet-tables-migration.bat
   ```

7. **`manual-fix-migration.bat`** - Provides manual SQL commands to fix migration history
   ```bash
   manual-fix-migration.bat
   ```

## 📋 Usage Examples

### Development Workflow

```bash
# Navigate to scripts/migrations folder
cd scripts/migrations

# Create a new migration for development
create-sqlite-migration.bat "AddNewFeature"

# Apply the migration to development database
update-sqlite-database.bat
```

### Production Workflow

```bash
# Navigate to scripts/migrations folder
cd scripts/migrations

# Create a new migration for production
create-sqlserver-migration.bat "AddNewFeature"

# Apply the migration to production database
update-sqlserver-database.bat
```

## ⚠️ Important Notes

- Always test migrations in development first
- The migration files are shared between environments
- The conditional logic in migrations handles both SQLite and SQL Server
- Production migrations use Azure AD authentication
- Development migrations use SQLite file database

## 🔧 Troubleshooting

### Connection Errors

If you get connection errors:

1. Make sure you're logged into Azure CLI: `az login`
2. Check your Azure SQL firewall settings
3. Verify the connection strings in appsettings files
4. Ensure you're in the correct directory when running scripts

### Migration Errors

If you get "There is already an object named 'AspNetRoles'" error:

1. **Quick Fix**: Run `fix-aspnet-tables-migration.bat`
2. **Manual Fix**: Run `manual-fix-migration.bat` and follow the SQL commands
3. **General Fix**: Run `fix-migration-history.bat` for other migration issues

### Common Migration Issues

- **Tables already exist**: Use the fix scripts above
- **Migration history out of sync**: Use `fix-migration-history.bat`
- **Missing migration records**: Use `manual-fix-migration.bat`

## 📝 Quick Reference

| Action           | Development                          | Production                              |
| ---------------- | ------------------------------------ | --------------------------------------- |
| Create Migration | `create-sqlite-migration.bat "Name"` | `create-sqlserver-migration.bat "Name"` |
| Apply Migration  | `update-sqlite-database.bat`         | `update-sqlserver-database.bat`         |
