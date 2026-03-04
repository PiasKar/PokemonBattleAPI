using PokemonBattle.API.Models;

namespace PokemonBattle.API.Models;

public class BattleEntity
{
    public int Id { get; set; }

    public int PokemonAId { get; set; }
    public PokemonEntity PokemonA { get; set; } = null!;

    public int PokemonBId { get; set; }
    public PokemonEntity PokemonB { get; set; } = null!;

    public int? WinnerPokemonId { get; set; }
    public PokemonEntity? WinnerPokemon { get; set; }

    public int EffectivePowerA { get; set; }
    public int EffectivePowerB { get; set; }

    public string Reason { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
