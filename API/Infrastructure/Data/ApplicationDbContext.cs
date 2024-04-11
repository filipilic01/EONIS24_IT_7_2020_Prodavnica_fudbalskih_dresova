using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Jersey> Jerseys { get; set; }

        public DbSet<JerseySize> JerseySizes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasIndex(e => e.AdminEmail)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(e => e.AdminUserName)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(e => e.CustomerEmail)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(e => e.CustomerUserName)
                .IsUnique();

            modelBuilder.Entity<Jersey>()
                .ToTable(table =>
                {
                    table.HasCheckConstraint("CHK_Status", "Status IN ('na stanju', 'nedostupno')");
                });

            modelBuilder.Entity<Jersey>()
                .HasOne(a => a.Admin)
                .WithMany(j => j.Jerseys)
                .HasForeignKey(a => a.AdminId);

            modelBuilder.Entity<JerseySize>()
                .HasOne(j => j.Jersey)
                .WithMany(s => s.JerseySizes)
                .HasForeignKey(j => j.JerseyId);

            modelBuilder.Entity<Order>()
                .HasOne(c => c.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(o => o.OrderId);


            modelBuilder.Entity<JerseySize>()
                .HasOne(oi => oi.OrderItem)
                .WithOne(s => s.JerseySize)
                .HasForeignKey<OrderItem>(oi => oi.JerseySizeId);
                

        }
    }
}
