using PokemonBattle.API.Services;

public class DiceRoller : IDiceRoller
{
    private readonly Random _random = new();

    public int RollD20() => _random.Next(1, 21); // 1..20
}
