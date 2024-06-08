using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

public static class GameActions
{
    public static int GetSecondsSinceLastEncounter(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return Math.Min((int)diff.TotalSeconds, Conts.MaxIdleSecond);
    }

    public static IdleRewards RunEncounters(float elapedsSeconds)
    {
        int encounters = GetEncounters(elapedsSeconds);
        if (encounters == 0) return new IdleRewards();
        return encounters > 1 ? TriggerEncounter() : TriggerMultipleEncounters(encounters, elapedsSeconds);
    }



    public static int GetEncounters(float elapedsSeconds)
    {
        return (int)(elapedsSeconds / State.GetEncounterSpeed());
    }

    public static IdleRewards TriggerEncounter()
    {
        Elemental elemental = State.Maps.currentMap.GetEncounter();
        IdleRewards rewards = new IdleRewards();

        bool isCaught = elemental.Catch(/*add modifiers*/);
        if (isCaught)
        {
            bool isNewCatch = !State.Elementals.IsElementalRegistered(elemental.id);

            rewards.totalCatches = 1;
            rewards.experience = elemental.expGain;
            rewards.orbs = elemental.orbsGain;
            if (isNewCatch) rewards.newCatches.Add(elemental.id);
            if (!isNewCatch) rewards.elementalTokens.Add(elemental.id, 1);
        }

        return rewards;
    }

    public static IdleRewards TriggerMultipleEncounters(int multiplier, float elapedsSeconds)
    {
        IdleRewards rewards = new IdleRewards();

        ElementalEncounter[] elementalEncounters = State.Maps.currentMap.elementalEncounters;
        foreach (ElementalEncounter encounter in elementalEncounters)
        {
            Elemental elemental = State.Elementals.GetElement(encounter.elementalId);
            int apperences = (int)(encounter.encounterChance * multiplier);
            int catches = (int)(apperences * elemental.catchRate);

            if (catches == 0) break;

            if (!State.Elementals.IsElementalRegistered(elemental.id))
            {
                rewards.newCatches.Add(elemental.id);
                catches--;
            }

            rewards.elementalTokens.Add(elemental.id, catches);
            rewards.experience += elemental.expGain * catches;
            rewards.orbs += elemental.orbsGain * catches;
            rewards.totalCatches += catches;
        }

        rewards.gold =  GoldMine.GetTotalGoldFromAllMaps() * (int)(elapedsSeconds / GoldMine.incomeLoopSeconds);
        rewards.essence = EssenceLab.GetTotalEssenceFromAllMaps() * (int)(elapedsSeconds / EssenceLab.incomeLoopSeconds);

        return rewards;
    }

    public static void EarnRewardOfEncounters(IdleRewards rewards)
    {
        rewards.newCatches.ForEach(c => State.Elementals.MarkElementalAsCaught(c));
        rewards.elementalTokens.ToList().ForEach(c => State.Elementals.UpdateElementalTokens(c.Key, c.Value));
        State.GainExperience(rewards.experience);
        State.UpdateEssence(rewards.orbs);
    }
}

public class IdleRewards
{
    public int totalCatches = 0;
    public int orbs = 0;
    public int gold = 0;
    public int essence = 0;
    public int experience = 0;
    public List<ElementalId> newCatches = new List<ElementalId>();
    public Dictionary<ElementalId, int> elementalTokens = new Dictionary<ElementalId, int>();
}