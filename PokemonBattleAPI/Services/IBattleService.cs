using PokemonBattle.API.Models;

namespace PokemonBattle.API.Services;

public interface IBattleService
{
    BattleOutcome Fight(PokemonEntity a, PokemonEntity b);
}