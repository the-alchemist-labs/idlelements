using System;
using Newtonsoft.Json;

public enum EncounterState
{
    InProgress,
    Caught,
    OutOfTries,
}

[Serializable]
public class Encounter
{
    public static int MAX_CATCH_TRIES { get { return 3; } }

    public ElementalId EncounterId { get; private set; }
    public int Tries { get; private set; }
    public EncounterState state { get; private set; }

    public Encounter()
    {
        EncounterId = ElementalId.None;
        Tries = 0;
    }

    [JsonConstructor]
    public Encounter(ElementalId EncounterId, int Tries, EncounterState state)
    {
        this.EncounterId = EncounterId;
        this.Tries = Tries;
        this.state = state;
    }

    public void SetNewEncounter(ElementalId encounterId)
    {
        EncounterId = encounterId;
        Tries = 0;
        state = EncounterState.InProgress;
    }

    public void UseTry(bool isCaught)
    {
        Tries++;

        if (isCaught)
        {
            state = EncounterState.Caught;
            return;
        }

        if (Tries >= MAX_CATCH_TRIES)
        {
            state = EncounterState.OutOfTries;
        }
    }

    public bool HasRemainingTries()
    {
        return state != EncounterState.Caught;
    }
}