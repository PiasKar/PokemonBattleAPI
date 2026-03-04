using Microsoft.EntityFrameworkCore;
using PokemonBattle.API.Models;


namespace PokemonBattle.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PokemonEntity> Pokemons => Set<PokemonEntity>();
    public DbSet<BattleEntity> Battles => Set<BattleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonEntity>()
            .HasIndex(p => p.Name)
            .IsUnique();
        
        modelBuilder.Entity<PokemonEntity>()
        .HasIndex(p => p.Name)
        .IsUnique();

        modelBuilder.Entity<BattleEntity>()
            .HasOne(b => b.PokemonA)
            .WithMany()
            .HasForeignKey(b => b.PokemonAId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BattleEntity>()
            .HasOne(b => b.PokemonB)
            .WithMany()
            .HasForeignKey(b => b.PokemonBId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BattleEntity>()
            .HasOne(b => b.WinnerPokemon)
            .WithMany()
            .HasForeignKey(b => b.WinnerPokemonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}