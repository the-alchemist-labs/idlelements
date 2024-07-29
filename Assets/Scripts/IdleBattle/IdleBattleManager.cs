using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class IdleBattleManager : MonoBehaviour
{
    public static IdleBattleManager Instance;
    public int CurrentStage { get; private set; }
    public Dictionary<int, List<MinimentalId>> Stages { get; private set; }

    [SerializeField] private GameObject _skillPrefab;

    private ObjectPool<GameObject> _pool;

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

        // Temp here
        Stages = new Dictionary<int, List<MinimentalId>>{
            { 1, new List<MinimentalId>() { MinimentalId.FireMeele, MinimentalId.FireMeele, MinimentalId.FireRanged} },
            { 2, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged} },
            { 3, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged} },
            { 4, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged} },
            { 5, new List<MinimentalId>() { MinimentalId.FireRanged, MinimentalId.FireRanged, MinimentalId.FireRanged} },
        };

        _pool = new ObjectPool<GameObject>(
              createFunc: () => Instantiate(_skillPrefab),
              actionOnGet: GetInstance,
              actionOnRelease: ReleaseInstance,
              actionOnDestroy: obj => Destroy(obj),
              collectionCheck: true,
              defaultCapacity: 25,
              maxSize: 35
          );
    }

    public void ActivateSkill(Vector3 spawnPosition, Vector2 targetPosition, ElementalSkill skill, int power, string targetTag)
    {
        GameObject skillEffect = _pool.Get();
        skillEffect.transform.position = spawnPosition;
        skillEffect.GetComponent<SkillEffectPrefab>().Initialize(targetPosition, skill, power, targetTag);
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
}
