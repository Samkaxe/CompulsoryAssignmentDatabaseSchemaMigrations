# CompulsoryAssignmentDatabaseSchemaMigrations

# Manual Migration Section

## Creating the Database Schema

We initialized our database schema using SQL scripts to create tables and define relationships. Below are the steps and corresponding SQL commands used:

###  Table Creation

```sql
CREATE TABLE Products (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Price REAL NOT NULL
);
```


```sql
CREATE TABLE Categories (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);
```
```sql
CREATE TABLE Products(
Id INTEGER PRIMARY KEY,
Name TEXT NOT NULL,
Price REAL NOT NULL,
category_id INTEGER,
FOREIGN KEY (category_id) REFERENCES Categories(Id)
);
```

## Testing and Validation with Mock Data

### Mock data was inserted to test the integrity and relationships of the tables:

```sql
-- Categories
INSERT INTO Categories (Name) VALUES ('Electronics'), ('Books'), ('Home Appliances');

-- Products
INSERT INTO Products (Name, Price, category_id) VALUES
                                                    ('Laptop', 1200.00, 1),
                                                    ('Smartphone', 800.00, 1),
                                                    ('Database Systems', 59.99, 2),
                                                    ('Python Programming', 39.99, 2),
                                                    ('Microwave Oven', 99.99, 3),
                                                    ('Vacuum Cleaner', 149.99, 3);

-- ProductRatings
INSERT INTO ProductRatings (ProductId, Rating, Review) VALUES
                                                           (1, 5, 'Excellent laptop with high performance.'),
                                                           (1, 4, 'Good but battery life could be better.'),
                                                           (2, 4, 'Very reliable and fast.'),
                                                           (3, 5, 'Comprehensive and detailed. Highly recommended.'),
                                                           (4, 5, 'Great for beginners and advanced programmers alike.');
```

### Rollback Plan
### Sequential Dropping of Tables
#### Given SQLite's referential integrity constraints, we adhered to a specific sequence for dropping tables:
### during the creation each command was in single file to control which table should be droped but we need to take the 
### foreign key in consideration 
```sql
-- Drop ProductRatings
DROP TABLE IF EXISTS ProductRatings;

-- Drop Products
DROP TABLE IF EXISTS Products;

-- Drop Categories
DROP TABLE IF EXISTS Categories;
```

## Alternative Approach: Disabling Foreign Key Checks

### For scenarios permitting temporary disabling of foreign key checks, the following command facilitates a simpler rollback process:

PRAGMA foreign_keys = OFF;


# Entity Framework Core Migration Guide

This guide outlines the process of managing database schema changes in a .NET application using Entity Framework
Core (EF Core). The steps include library setup, creating table models and DbContext, generating and applying migrations, and handling migration history.

## Library Setup

To work with EF Core migrations, first ensure the necessary EF Core libraries are included in the project:

- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Sqlite`

These packages enable the use of EF Core with SQLite databases and provide tools necessary for generating and applying migrations.

## Defining Table Models and DbContext

### Table Models

Define your table models within the application. These are C# classes that represent the tables in your database. For example:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

## DbContext
### Create a DbContext that represents a session with the database, allowing you to query and save instances of your entities. Include DbSet properties for each model:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=app.db");
}
```

## Generating and Applying Migrations
### Generating Migrations
### To create a new migration based on the changes made to your models or DbContext, use the following command:

dotnet ef migrations add InitialMigration --context AppDbContext

### This command generates migration files that include the necessary commands to update the database schema.


## Applying Migrations
### apply the migrations to your database to create or update the schema:

dotnet ef database update --context AppDbContext

## Migration History
 Each time a migration occurs, EF Core logs the migration history in a special table in your database 
 (by default, named __EFMigrationsHistory). This table includes records like 20240303160553_InitialMigration,7.0.16 and
 20240305161738_AddTestTable,7.0.16, indicating the migration name and the EF Core version used

## Rolling Back Migrations

To roll back to a specific migration, use the database update command with the target migration name:

#### lets say we need to drop the table test we just need to grab the name of the migration of the initial setup and use the following command 
#### this will remove the table test , and roll back to privous state 
dotnet ef database update 20240303160553_InitialMigration --context AppDbContext