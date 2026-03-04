using PokemonBattle.API.Models;

namespace PokemonBattle.API.Services;

public class BattleService : IBattleService
{
    private readonly IDiceRoller _dice;

    public BattleService(IDiceRoller dice)
    {
        _dice = dice;
    }

    public BattleOutcome Fight(PokemonEntity a, PokemonEntity b)
    {
        var rollA = _dice.RollD20();
        var rollB = _dice.RollD20();

        var effectiveA = a.Power + (rollA * 2) + CritBonus(rollA);
        var effectiveB = b.Power + (rollB * 2) + CritBonus(rollB);

        int? winnerId = null;
        string reason;

        if (effectiveA > effectiveB)
        {
            winnerId = a.Id;
            reason = $"A wins (roll {rollA} vs {rollB})";
        }
        else if (effectiveB > effectiveA)
        {
            winnerId = b.Id;
            reason = $"B wins (roll {rollA} vs {rollB})";
        }
        else
        {
            if (a.Speed > b.Speed)
            {
                winnerId = a.Id;
                reason = $"Speed tie-break (roll {rollA} vs {rollB})";
            }
            else if (b.Speed > a.Speed)
            {
                winnerId = b.Id;
                reason = $"Speed tie-break (roll {rollA} vs {rollB})";
            }
            else
            {
                reason = $"Draw (roll {rollA} vs {rollB})";
            }
        }

        return new BattleOutcome
        {
            EffectivePowerA = effectiveA,
            EffectivePowerB = effectiveB,
            WinnerPokemonId = winnerId,
            Reason = reason
        };
    }

    private static int CritBonus(int roll)
    {
        if (roll == 20) return 30;
        if (roll == 1) return -20;
        return 0;
    }
}