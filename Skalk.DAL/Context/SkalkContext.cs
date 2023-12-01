using Skalk.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class SkalkContext : DbContext
{
    public SkalkContext(DbContextOptions<SkalkContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
   
    public DbSet<User> Users { get; set; }

    public DbSet<ItemShoppingCart> ItemShoppingCarts { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
}
