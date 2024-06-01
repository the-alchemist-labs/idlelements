using System.Collections.Generic;
using UnityEngine;

public static class GameActions
{
    // add on click to call the terigger encounter on screen (with bonus to the terain he clicked)
    public static void TriggerEncounter()
    {
        Elemental elemental = State.Maps.GetMap(State.currentMap).GetEncounter();
        State.Elementals.MarkElementalAsSeen(elemental.id);

        bool isCaught = elemental.Catch(/*add modifiers*/);
        if (isCaught)
        {
            if (State.Elementals.IsElementalRegistered(elemental.id))
            {
                State.Elementals.UpdateElementalTokens(elemental.id, 1);
            }
            else
            {
                State.Elementals.MarkElementalAsCaught(elemental.id);
            }

            State.Maps.UpdateMapProgression(1);
            State.UpdateEssence(elemental.essenceGain);
            State.GainExperience(elemental.expGain);
        }

        Debug.Log($"A wild {elemental.name} apperead, it was {(isCaught ? "" : "not")} caught");
    }

    public static void TriggerMultipleEncounters(int multiplier)
    {
        ElementalEncounter[] encounters = State.Maps.GetMap(State.currentMap).elementalEncounters;
        foreach (ElementalEncounter encounter in encounters)
        {
            Elemental elemental = State.Elementals.GetElement(encounter.elementalId);
            int apperences = (int)(encounter.encounterChance * multiplier);
            int catches = (int)(apperences * elemental.catchRate);

            Debug.Log($"{apperences} {elemental.name}s appered! {catches} caught");
            
            State.Elementals.UpdateElementalTokens(elemental.id, catches);
            State.GainExperience(elemental.expGain * catches);
            State.UpdateEssence(elemental.essenceGain * catches);
            State.Maps.UpdateMapProgression(catches);
        }
    }

}