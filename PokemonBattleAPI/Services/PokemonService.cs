using Microsoft.EntityFrameworkCore;
using PokemonBattle.API.Data;
using PokemonBattle.API.Models;

namespace PokemonBattle.API.Services;

public class PokemonService : IPokemonService
{
    private readonly AppDbContext _db;
    private readonly IPokeApiClient _api;

    public PokemonService(AppDbContext db, IPokeApiClient api)
    {
        _db = db;
        _api = api;
    }

    public async Task<PokemonEntity?> ImportFromApiAsync(string nameOrId)
    {
        // cache: jeśli istnieje, nie dubluj
        var existing = await _db.Pokemons.FirstOrDefaultAsync(p => p.Name == nameOrId.ToLower());
        if (existing != null) return existing;

        var dto = await _api.GetPokemonAsync(nameOrId);
        if (dto == null) return null;

        var entity = new PokemonEntity
        {
            PokeApiId = dto.PokeApiId,
            Name = dto.Name.ToLower(),
            PrimaryType = dto.PrimaryType,
            SecondaryType = dto.SecondaryType,
            Attack = dto.Attack,
            Defense = dto.Defense,
            Speed = dto.Speed,
            Power = dto.Attack + dto.Defense + dto.Speed,
            ImportedAt = DateTime.UtcNow
        };

        _db.Pokemons.Add(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public Task<List<PokemonEntity>> GetAllAsync()
        => _db.Pokemons.OrderBy(p => p.Id).ToListAsync();

    public async Task Delete(int id)
    {
        var pokemon = await GetByIdAsync(id);
        _db.Pokemons.Remove(pokemon);
        await _db.SaveChangesAsync();

    }

    public async Task<PokemonEntity?> GetByIdAsync(int id)
    {
        var pokemon = await _db.Pokemons.FirstOrDefaultAsync(p => p.Id == id);

        if (pokemon == null)
        {
            throw new Exception("Pokemon not found");
        }

        return pokemon;
    }
}