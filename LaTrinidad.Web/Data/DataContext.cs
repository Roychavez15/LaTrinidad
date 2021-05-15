using LaTrinidad.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LaTrinidad.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<PacientEntity> PacientEntities { get; set; }
        public DbSet<scheduleEntity> ScheduleEntities { get; set; }

        //public DbSet<Provincia> Provincias { get; set; }
        //public DbSet<Ciudad> Ciudades { get; set; }
        //public DbSet<Oficina> Oficinas { get; set; }
        //public DbSet<UsersCampaniaEntity> UsersCampaniaEntity { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Campania>()
            //    .HasIndex(t => t.Nombre)
            //    .IsUnique();
        }

    }
}
