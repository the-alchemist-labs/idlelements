using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _idleBattleEnemyPrefab;
    [SerializeField] private Transform[] _spawnLocations = new Transform[5];
    [SerializeField] private int _enemiesCounter;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_idleBattleEnemyPrefab),
            actionOnGet: GetInstance,
            actionOnRelease: ReleaseInstance,
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: 15,
            maxSize: 25
        );
    }

    void Start()
    {
        StartStage(IdleBattleManager.Instance.CurrentStage);
    }

    private void GetInstance(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<BattleEnemyPrefab>().OnDefeat += onEnemeyDeath;
    }

    private void ReleaseInstance(GameObject obj)
    {
        obj.GetComponent<BattleEnemyPrefab>().OnDefeat -= onEnemeyDeath;
        obj.SetActive(false);
    }

    private GameObject SetEnemy(MinimentalId id, int level)
    {
        Transform spawLocation = _spawnLocations[Random.Range(0, _spawnLocations.Length)];
        GameObject obj = _pool.Get();
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.position = spawLocation.position;

        BattleEnemyPrefab prefabScript = obj.GetComponent<BattleEnemyPrefab>();
        prefabScript.InitializeEnemy(id, level);
        return obj;
    }

    private void StartStage(int waveNum)
    {
        List<MinimentalId> stage = IdleBattleManager.Instance.GetStage(waveNum);
        foreach (MinimentalId stageMinimemtalId in stage)
        {
            SetEnemy(stageMinimemtalId, waveNum);
        }
        _enemiesCounter = stage.Count;
    }

    private void onEnemeyDeath(GameObject obj)
    {
        _pool.Release(obj);
        _enemiesCounter--;

        if (_enemiesCounter == 0)
        {
            IdleBattleManager.Instance.IncrementCurrentStage();
            StartStage(IdleBattleManager.Instance.CurrentStage);
        }
    }
}

