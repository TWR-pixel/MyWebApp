using Microsoft.EntityFrameworkCore;
using MyWebApp.Data.Entities;

namespace MyWebApp.Data;

public sealed class NorthwindContext : DbContext
{
    public DbSet<Image> Images { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    private readonly string _connStr;

    public NorthwindContext(string connectionString)
    {
        _connStr = connectionString;
        Database.EnsureCreated();

        //DatabaseInitializer.Init(this);
    }

    public NorthwindContext(DbContextOptions o) : base(o)
    {
        _connStr = "Data source = northwind.db";
        Database.EnsureCreated();
        //DatabaseInitializer.Init(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connStr);
    }

}