using UnityEngine;

public static class GameActions
{
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
}