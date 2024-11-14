using APIMarafon.NET_Core.Models;
using ApiMarafons.Table;
using Microsoft.EntityFrameworkCore;


namespace TodoApi.Models
{
    public class MARAFON_DBContext:DbContext
    {  
            public MARAFON_DBContext(DbContextOptions<MARAFON_DBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MarafonDB>().Property(t =>  t.Name).IsRequired();
        }

        public virtual DbSet<MARAFON> MARAFON { get; set; }
        public virtual DbSet<MARAFON_UCHASTIE> MARAFON_UCHASTIE { get; set; }
        public virtual DbSet<SPORTMENS> SPORTMENS { get; set; }
        public virtual DbSet<TYPE> TYPE { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<ZRITELI> ZRITELI { get; set; }
        public DbSet<MarafonDB> MarafonDBs { get; set; } = null!;

    }
}
