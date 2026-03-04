using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonBattle.API.Data;
using PokemonBattle.API.Models;
using PokemonBattle.API.Services;
using PokemonBattle.API.Models;

namespace PokemonBattle.API.Controllers;

[ApiController]
[Route("api/pokemons")]
public class PokemonsController : ControllerBase
{
    private readonly IPokeApiClient _pokeApi;
    private readonly IPokemonService _pokemonService;
    private readonly AppDbContext _db;
    private readonly IBattleService _battleService;

    public PokemonsController(
        IPokeApiClient pokeApi,
        IPokemonService pokemonService,
        AppDbContext db,
        IBattleService battleService)
    {
        _pokeApi = pokeApi;
        _pokemonService = pokemonService;
        _db = db;
        _battleService = battleService;
    }

    [HttpGet("api/{nameOrId}")]
    public async Task<IActionResult> GetFromApi(string nameOrId)
    {
        var dto = await _pokeApi.GetPokemonAsync(nameOrId);
        if (dto == null) return NotFound("Pokemon not found in PokeAPI.");
        return Ok(dto);
    }

    [HttpPost("import/{nameOrId}")]
    public async Task<IActionResult> Import(string nameOrId)
    {
        var entity = await _pokemonService.ImportFromApiAsync(nameOrId);
        if (entity == null) return NotFound("Pokemon not found in PokeAPI.");
        return Ok(entity);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFromDb()
    {
        var all = await _pokemonService.GetAllAsync();
        return Ok(all);
    }

    [HttpDelete("api/{id}")]
    public async Task<IActionResult> DeleteFromDb(int id)
    {
        await _pokemonService.Delete(id);
        return NoContent();
    }

    [HttpGet("api/PokemonById/{id}")]
    public async Task<ActionResult> GetByIdFromDb(int id)
    {
        var pokemon = await _pokemonService.GetByIdAsync(id);
        return Ok(pokemon);
    }

    // POST /api/pokemons/fight?pokemonAId=1&pokemonBId=2
    [HttpPost("fight")]
    public async Task<IActionResult> Fight([FromQuery] int pokemonAId, [FromQuery] int pokemonBId)
    {
        if (pokemonAId == pokemonBId)
            return BadRequest("Choose two different pokemons.");

        var a = await _db.Pokemons.FirstOrDefaultAsync(p => p.Id == pokemonAId);
        var b = await _db.Pokemons.FirstOrDefaultAsync(p => p.Id == pokemonBId);

        if (a == null || b == null)
            return NotFound("One or both pokemons not found in database.");

        var outcome = _battleService.Fight(a, b);

        var battle = new BattleEntity
        {
            PokemonAId = a.Id,
            PokemonBId = b.Id,
            WinnerPokemonId = outcome.WinnerPokemonId,
            EffectivePowerA = outcome.EffectivePowerA,
            EffectivePowerB = outcome.EffectivePowerB,
            Reason = outcome.Reason,
            CreatedAt = DateTime.UtcNow
        };

        _db.Battles.Add(battle);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            battle.Id,
            PokemonA = a.Name,
            PokemonB = b.Name,
            battle.EffectivePowerA,
            battle.EffectivePowerB,
            Winner = battle.WinnerPokemonId == null
                ? "Draw"
                : (battle.WinnerPokemonId == a.Id ? a.Name : b.Name),
            battle.Reason,
            battle.CreatedAt
        });
    }
}