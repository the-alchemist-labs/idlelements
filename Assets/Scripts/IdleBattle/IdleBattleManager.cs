using System.Collections.Generic;
using UnityEngine;

public class IdleBattleManager : MonoBehaviour
{
    public static IdleBattleManager Instance;
    public Dictionary<int, List<MinimentalId>> Stages { get; private set; }
    public int CurrentStage { get; private set; }
    public GameObject prefab;

    void Awake()
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
        IdleBattleManagerState state = DataService.Instance.LoadData<IdleBattleManagerState>(FileName.IdleBattleManagerState, true);
        CurrentStage = state.CurrentStage;
        Stages = new Dictionary<int, List<MinimentalId>>{
            { 1, new List<MinimentalId>() { MinimentalId.FireMeele, MinimentalId.FireMeele, MinimentalId.FireRanged} },
            { 2, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged} },

        };
    }

    public string GetStageName()
    {
        return $"{CurrentStage / 10}-{CurrentStage % 10}";
    }
    public List<MinimentalId> GetStage(int stageNum)
    {
        if (Stages.TryGetValue(stageNum, out List<MinimentalId> elementalDictionary))
        {
            return elementalDictionary;
        }
        return new List<MinimentalId>();
    }

    public int IncrementCurrentStage()
    {
        return ++CurrentStage;
    }
}
