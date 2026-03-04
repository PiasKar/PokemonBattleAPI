namespace PokemonBattle.API.Models;

public class BattleOutcome
{
    public int EffectivePowerA { get; set; }
    public int EffectivePowerB { get; set; }
    public int? WinnerPokemonId { get; set; }
    public string Reason { get; set; } = "";
}
