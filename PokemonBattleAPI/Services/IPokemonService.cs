using PokemonBattle.API.Models;

namespace PokemonBattle.API.Services;

public interface IPokemonService
{
    Task<PokemonEntity?> ImportFromApiAsync(string nameOrId);
    Task<List<PokemonEntity>> GetAllAsync();
    Task<PokemonEntity?> GetByIdAsync(int id);
    Task Delete(int id);
}