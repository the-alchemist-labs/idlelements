using System;
using System.Collections.Generic;
using System.Linq;

public static class GameActions
{
    // add on click to call the trigger encounter on screen (with bonus to the terain he clicked)
    public static IdleRewards TriggerEncounter()
    {
        Elemental elemental = State.Maps.GetMap(State.currentMap).GetEncounter();
        IdleRewards rewards = new IdleRewards();

        bool isCaught = elemental.Catch(/*add modifiers*/);
        if (isCaught)
        {
            bool isNewCatch = !State.Elementals.IsElementalRegistered(elemental.id);

            rewards.totalCatches = 1;
            rewards.experience = elemental.expGain;
            rewards.essence = elemental.essenceGain;
            if (isNewCatch) rewards.newCatches.Add(elemental.id);
            if (!isNewCatch) rewards.elementalTokens.Add(elemental.id, 1);
        }

        return rewards;
    }

    public static IdleRewards TriggerMultipleEncounters(int multiplier)
    {

        IdleRewards rewards = new IdleRewards();

        ElementalEncounter[] elementalEncounters = State.Maps.GetMap(State.currentMap).elementalEncounters;
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
            rewards.essence += elemental.essenceGain * catches;
            rewards.totalCatches += catches;
        }

        return rewards;
    }


    public static void EarnRewardOfEncounters(IdleRewards rewards)
    {
        rewards.newCatches.ForEach(c => State.Elementals.MarkElementalAsCaught(c));
        rewards.elementalTokens.ToList().ForEach(c => State.Elementals.UpdateElementalTokens(c.Key, c.Value));
        State.GainExperience(rewards.experience);
        State.UpdateEssence(rewards.essence);
        State.Maps.UpdateMapProgression(rewards.totalCatches);
        State.UpdateLastEncounter(DateTime.Now);
    }
}

public class IdleRewards
{
    public int totalCatches = 0;
    public int essence = 0;
    public int experience = 0;
    public List<ElementalId> newCatches = new List<ElementalId>();
    public Dictionary<ElementalId, int> elementalTokens = new Dictionary<ElementalId, int>();
}