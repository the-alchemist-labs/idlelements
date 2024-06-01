using UnityEngine;

public static class GameActions
{
    public static void TriggerEncounter()
    {
        Elemental elemental = Maps.GetMap(State.currentMap).GetEncounter();
        State.MarkElementalAsSeen(elemental.id);

        bool isCaught = elemental.Catch(/*add modifiers*/);
        if (isCaught)
        {
            if (State.IsElementalRegistered(elemental.id))
            {
                State.UpdateElementalTokens(elemental.id, 1);
            }
            else
            {
                State.MarkElementalAsCaught(elemental.id);
            }

            State.UpdateMapProgression(1);
            State.UpdateEssence(elemental.essenceGain);
            State.GainExperience(elemental.expGain);
        }

        Debug.Log($"A wild {elemental.name} apperead, it was {(isCaught ? "" : "not")} caught");
    }
}