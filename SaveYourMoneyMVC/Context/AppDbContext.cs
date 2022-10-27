using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SaveYourMoneyMVC.Entities;

namespace SaveYourMoneyMVC.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Gasto>().HasOne(g => g.File).WithOne(f => f.Gasto).HasForeignKey<FileEntity>(f => f.GastoId).OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
