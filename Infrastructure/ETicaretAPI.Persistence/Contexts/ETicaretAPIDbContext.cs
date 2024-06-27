using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Contexts
{
    public class ETicaretAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ETicaretAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet <Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasKey(o => o.Id);
            builder.Entity<Order>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Basket)
                .HasForeignKey<Order>(o =>o.Id);
            builder.Entity<Order>()
                .HasOne(o => o.CompletedOrder)
                .WithOne(c => c.Order)
                .HasForeignKey<CompletedOrder>(c => c.OrderId);
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker = Entityler üzerinden yeni eklenen vey değişiklik yapılan verinin yakalanmasını sağlayan propertydir. Update operasyonlarında Trcak edilen verileri yapalayıp elde etmemizi sağlar. İnsert edilen dışında, insert edilen nesne henüz daha olmadığı için yakalanamaz.
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch //Discard işlemi yapıyoruz, yani herhangi bir atama işlemi yapılmasını istemiyoruz.
                {
                    EntityState.Added=>data.Entity.CreatedDate=DateTime.UtcNow,
                    EntityState.Modified =>data.Entity.UpdatedDate=DateTime.UtcNow,
                    _=>DateTime.UtcNow
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
