using System.Collections.Generic;
using UnityEngine;

public class StageCatalog : MonoBehaviour
{
    public static StageCatalog Instance { get; private set; }
    public Dictionary<int, Dictionary<ElementalId, int>> Stages { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        Stages = DataService.Instance.LoadData<Dictionary<int, Dictionary<ElementalId, int>>>(FileName.StageCatalog, false);
    }

    public Dictionary<ElementalId, int> GetStage(int stageNum)
    {
        if (Stages.TryGetValue(stageNum, out Dictionary<ElementalId, int> elementalDictionary))
        {
            return elementalDictionary;
        }
        return new Dictionary<ElementalId, int>();
    }
}