using JuniorGrid.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JuniorGrid.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Jugador> Jugadores { get; set; } = null!;
        public DbSet<Criterio> Criterios { get; set; } = null!;
        public DbSet<Top10Criterio> Top10Criterios { get; set; } = null!; 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Criterio>(entity =>
            {
                entity.ToTable("GridCriterios");
                entity.HasKey(c => c.CriterioId);
                entity.Property(c => c.Etiqueta).IsRequired().HasMaxLength(100);
            });

         
            modelBuilder.Entity<Top10Criterio>(entity =>
            {
                entity.ToTable("Top10Criterios");
                entity.HasKey(c => c.CriterioId);
                entity.Property(c => c.Etiqueta).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Tipo).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Jugador>(entity =>
            {
                entity.ToTable("Jugadores");
                entity.HasKey(j => j.JugadorId);
                entity.Property(j => j.Nombre).IsRequired().HasMaxLength(100);
            });
        }
    }
}