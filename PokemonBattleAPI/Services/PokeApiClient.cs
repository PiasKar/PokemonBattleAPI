using System.Text.Json;
using PokemonBattle.API.Models;

namespace PokemonBattle.API.Services;

public class PokeApiClient : IPokeApiClient
{
    private readonly HttpClient _http;

    public PokeApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<PokeApiPokemonDto?> GetPokemonAsync(string nameOrId)
    {
        var url = $"https://pokeapi.co/api/v2/pokemon/{nameOrId.ToLower()}";
        var res = await _http.GetAsync(url);

        if (!res.IsSuccessStatusCode)
            return null;

        var json = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Id + name
        var id = root.GetProperty("id").GetInt32();
        var name = root.GetProperty("name").GetString() ?? "";

        // Types (0..2)
        var typesArray = root.GetProperty("types");
        string primaryType = typesArray[0].GetProperty("type").GetProperty("name").GetString() ?? "";
        string? secondaryType = typesArray.GetArrayLength() > 1
            ? typesArray[1].GetProperty("type").GetProperty("name").GetString()
            : null;

        // Stats: znajdź po nazwie (bez zgadywania indeksów)
        int GetStat(string statName)
        {
            foreach (var s in root.GetProperty("stats").EnumerateArray())
            {
                var n = s.GetProperty("stat").GetProperty("name").GetString();
                if (n == statName)
                    return s.GetProperty("base_stat").GetInt32();
            }
            return 0;
        }

        return new PokeApiPokemonDto
        {
            PokeApiId = id,
            Name = name,
            PrimaryType = primaryType,
            SecondaryType = secondaryType,
            Attack = GetStat("attack"),
            Defense = GetStat("defense"),
            Speed = GetStat("speed")
        };
    }
}