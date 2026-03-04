namespace PokemonBattle.API.Models;

public class PokeApiPokemonDto
{
    public int PokeApiId { get; set; }
    public string Name { get; set; } = "";
    public string PrimaryType { get; set; } = "";
    public string? SecondaryType { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
}