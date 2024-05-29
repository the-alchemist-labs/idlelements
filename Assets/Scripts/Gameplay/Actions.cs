using UnityEngine;

public static class GameActions
{
    public static void TriggerEncounter()
    {
        Elemental elemental = Maps.GetMap(State.currentMap).GetEncounter();
        elemental.MarkAsSeen();

        bool isCaught = elemental.Catch(/*add modifiers*/);
        if (isCaught)
        {
            State.UpdateEssence(elemental.essenceGain);
            State.GainExperience(elemental.expGain);
        }
        Elementals.Save();
        Debug.Log($"A wild {elemental.name} apperead, it was {(isCaught ? "" : "not")} caught\n Level: {State.level} Exp: {State.experience} Essence: {State.essence}");
    }
}