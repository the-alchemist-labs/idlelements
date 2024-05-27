using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    public static List<DeckRegistry> all { get; private set; }

    static Deck()
    {
        all = DataService.Instance.LoadData<List<DeckRegistry>>(FileName.Deck);
        FillMissingRegistries();
    }

    private static void FillMissingRegistries()
    {
        IEnumerable<ElementalId> allIds = Enum.GetValues(typeof(ElementalId)).Cast<ElementalId>();
        IEnumerable<ElementalId> missingIds = allIds.Except(all.Select(d => d.id));
        foreach (ElementalId id in missingIds)
        {
            all.Add(new DeckRegistry(id));
        }
    }

    public static DeckRegistry GetElementRegistry(ElementalId id)
    {
        return all.Find(el => el.id == id);
    }

    public static void RegisterElement(ElementalId id)
    {
        DeckRegistry reg = GetElementRegistry(id);

        if (reg == null || !reg.isRegistered)
        {
            reg.Register();
        }
        else
        {
            reg.UpdateTokensBy(1);
        }
        DataService.Instance.SaveData(FileName.Deck, all);
    }

    public static void UpdateTokensBy(ElementalId id, int updateBy)
    {
        DeckRegistry reg = GetElementRegistry(id);

        if (reg != null)
        {
            Debug.LogError($"Failed to update tokens of #{id}");
            return;
        }

        reg.UpdateTokensBy(updateBy);
        DataService.Instance.SaveData(FileName.Deck, all);
    }
}