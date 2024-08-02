using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class IdleBattleManager : MonoBehaviour
{
    public static IdleBattleManager Instance;
    public int CurrentStage { get; private set; }
    public DateTime LastRewardTimestamp { get; private set; }

    [SerializeField] private GameObject skillPrefab;

    private ObjectPool<GameObject> _pool;

    private Dictionary<ElementType, List<MinimentalId>> _stageMinimentalByType = new Dictionary<ElementType, List<MinimentalId>> {
        { ElementType.Fire, new List<MinimentalId>() { MinimentalId.FireMeele, MinimentalId.FireMeele, MinimentalId.FireMeele  } },
        { ElementType.Water, new List<MinimentalId>() {  MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged } },
        { ElementType.Earth, new List<MinimentalId>() { MinimentalId.FireMeele, MinimentalId.FireMeele, MinimentalId.FireMeele  } },
        { ElementType.Air, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged } },
        { ElementType.Ice, new List<MinimentalId>() { MinimentalId.FireMeele, MinimentalId.FireMeele, MinimentalId.FireMeele } },
        { ElementType.Lightning, new List<MinimentalId>() { MinimentalId.FireMeele, MinimentalId.FireMeele, MinimentalId.FireMeele } },
        { ElementType.Chaos, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged } }
    };

    private Dictionary<ElementType, MinimentalId> _stageBossByType = new Dictionary<ElementType, MinimentalId> {
        { ElementType.Fire, MinimentalId.FireBoss },
        { ElementType.Water,MinimentalId.FireBoss },
        { ElementType.Earth, MinimentalId.FireBoss },
        { ElementType.Air, MinimentalId.FireBoss },
        { ElementType.Ice, MinimentalId.FireBoss},
        { ElementType.Lightning, MinimentalId.FireBoss },
        { ElementType.Chaos, MinimentalId.FireBoss }
    };

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
        LastRewardTimestamp = state.LastRewardTimestamp;

        _pool = new ObjectPool<GameObject>(
              createFunc: () => Instantiate(skillPrefab),
              actionOnGet: GetInstance,
              actionOnRelease: ReleaseInstance,
              actionOnDestroy: obj => Destroy(obj),
              collectionCheck: true,
              defaultCapacity: 25,
              maxSize: 35
          );
    }

    public void UpdateLastRewardTimestam(DateTime date)
    {
        LastRewardTimestamp = date;
    }

    public void ActivateSkill(Vector3 spawnPosition, Vector2 targetPosition, Skill skill, int power, string targetTag)
    {
        GameObject skillEffect = _pool.Get();
        skillEffect.transform.position = spawnPosition;
        skillEffect.GetComponent<SkillEffectPrefab>().Initialize(targetPosition, skill, power, targetTag);
    }
    
    public string GetStageName()
    {
        int quotient = (CurrentStage - 1) / 10;
        int remainder = (CurrentStage - 1) % 10 + 1;
        return $"{quotient}-{remainder}";
    }

    public List<MinimentalId> GetStage(int stageNum)
    {
        ElementType stageType = GetElementTypeByStage(stageNum);

        if (stageNum % 10 == 0)
        {
            return new List<MinimentalId>() { _stageBossByType[stageType] };
        }

        return _stageMinimentalByType[stageType];
    }

    public int IncrementCurrentStage()
    {
        return ++CurrentStage;
    }

    public int GetStageEHP()
    {
        return GetStage(CurrentStage).Sum(m => GetMinimentalEHP(m, CurrentStage));
    }

    private void GetInstance(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<SkillEffectPrefab>().OnEffectCompleted += FinishEffect;
    }

    private void ReleaseInstance(GameObject obj)
    {
        obj.GetComponent<SkillEffectPrefab>().OnEffectCompleted -= FinishEffect;
        obj.SetActive(false);
    }

    private void FinishEffect(GameObject obj)
    {
        _pool.Release(obj);
    }

    private ElementType GetElementTypeByStage(int stageNum)
    {
        ElementType[] elementTypes = (ElementType[])Enum.GetValues(typeof(ElementType));
        int delta = stageNum % elementTypes.Length;
        return elementTypes[delta];
    }

    private int GetMinimentalEHP(MinimentalId id, int level)
    {
        Minimental minimental = ElementalCatalog.Instance.GetElemental(id);
        return (minimental.Stats.Hp + level) + (minimental.Stats.Defense + level);
    }
}
