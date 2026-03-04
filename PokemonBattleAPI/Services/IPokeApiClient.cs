using PokemonBattle.API.Models;

namespace PokemonBattle.API.Services;

public interface IPokeApiClient
{
    Task<PokeApiPokemonDto?> GetPokemonAsync(string nameOrId);
}