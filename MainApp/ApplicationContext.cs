using LabsDB.Entity;
using Microsoft.EntityFrameworkCore;

namespace MainApp;

public class ApplicationContext : DbContext
{
    private readonly bool _test;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        _test = false;
        Database.Migrate();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options, bool test) : base(options)
    {
        _test = test;
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<House> Houses { get; set; } = null!;
    public DbSet<Indication> Indications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_test)
        {
            base.OnModelCreating(modelBuilder);
            return;
        }

        var emp = new Employee {Id = 1, Login = "Test", Password = "Test"};
        var houses = new List<House>(new[]
        {
            new House {Id = 1},
            new House {Id = 2}
        });
        var inds = new List<object>(new[]
        {
            new {Id = 1, EmployeeId = 1, HouseId = 1, Value = 100D, Title = "Электричество", TimeStamp = DateTime.Now},
            new {Id = 2, EmployeeId = 1, HouseId = 1, Value = 200D, Title = "Вода", TimeStamp = DateTime.Now},
            new {Id = 3, EmployeeId = 1, HouseId = 2, Value = 200D, Title = "Электричество", TimeStamp = DateTime.Now},
            new {Id = 4, EmployeeId = 1, HouseId = 2, Value = 400D, Title = "Вода", TimeStamp = DateTime.Now}
        });
        modelBuilder.Entity<Employee>().HasData(emp);
        modelBuilder.Entity<House>().HasData(houses);
        modelBuilder.Entity<Indication>(i =>
        {
            i.HasOne(ind => ind.Employee)
                .WithMany(e => e.Indications)
                .HasForeignKey(ind => ind.EmployeeId);
            i.HasOne(ind => ind.House)
                .WithMany(e => e.Indications)
                .HasForeignKey(ind => ind.HouseId);
            i.HasData(inds);
        });
        base.OnModelCreating(modelBuilder);
    }
}