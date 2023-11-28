using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace RepositoryAndServices.Context
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImages> ItemImages { get; set; }
        public DbSet<CustomerItem> CustomerItems { get; set; }
        public DbSet<SupplierItem> SupplierItems { get; set; }
        public ApplicationDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
            .HasOne(b => b.UserType)
            .WithMany(b => b.Users)
            .HasForeignKey(p => p.UserTypeId)
            .IsRequired();

            modelBuilder.Entity<SupplierItem>()
            .HasOne(b => b.User)
            .WithMany(b => b.SupplierItems)
            .HasForeignKey(s => s.UserId)
            .IsRequired();

            modelBuilder.Entity<CustomerItem>()
            .HasOne(b => b.User)
            .WithMany(b => b.CustomerItems)
            .HasForeignKey(s => s.UserId)
            .IsRequired();

            modelBuilder.Entity<SupplierItem>()
            .HasOne(b => b.Item)
            .WithOne(b => b.SupplierItem)
            .HasForeignKey<SupplierItem>(s => s.ItemId)
            .IsRequired();

            modelBuilder.Entity<CustomerItem>()
            .HasOne(b => b.Item)
            .WithOne(b => b.CustomerItem)
            .HasForeignKey<CustomerItem>(s => s.ItemId)
            .IsRequired();

            modelBuilder.Entity<ItemImages>()
            .HasOne(b => b.Item)
            .WithMany(b => b.ItemImages)
            .HasForeignKey(s => s.ItemId)
            .IsRequired();

            modelBuilder.Entity<Category>()
            .HasMany(b => b.Items)
            .WithOne(b => b.Category)
            .HasForeignKey(s => s.CategoryId)
            .IsRequired();

        }
    }
}
