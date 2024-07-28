using System.Collections.Generic;
using UnityEngine;

public class StageMinimemtal
{
    public ElementalId Id;
    public int Level;
}

public class StageCatalog : MonoBehaviour
{
    public static StageCatalog Instance { get; private set; }
    public Dictionary<int, List<StageMinimemtal>> Stages { get; private set; }

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
        Stages = DataService.Instance.LoadData<Dictionary<int, List<StageMinimemtal>>>(FileName.StageCatalog, false);
    }

    public List<StageMinimemtal> GetStage(int stageNum)
    {
        if (Stages.TryGetValue(stageNum, out List<StageMinimemtal> elementalDictionary))
        {
            return elementalDictionary;
        }
        return new List<StageMinimemtal>();
    }
}