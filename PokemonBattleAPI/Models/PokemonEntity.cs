using System.ComponentModel.DataAnnotations;

namespace PokemonBattle.API.Models;

public class PokemonEntity
{
    public int Id { get; set; }

    public int PokeApiId { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = "";

    [MaxLength(30)]
    public string PrimaryType { get; set; } = "";

    [MaxLength(30)]
    public string? SecondaryType { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }

    public int Power { get; set; } // Attack + Defense + Speed
    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
}