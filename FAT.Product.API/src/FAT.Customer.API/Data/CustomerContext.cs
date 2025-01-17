﻿using FAT.Core.Data;
using FAT.Customers.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FAT.Customers.API.Data
{
    public sealed class CustomerContext : DbContext, IUnitOfWork
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Adresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
              foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}