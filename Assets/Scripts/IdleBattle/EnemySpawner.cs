using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject idleBattleEnemyPrefab;
    [SerializeField] private Transform[] spawnLocations = new Transform[3];
    [SerializeField] private int enemiesCounter;
    private ObjectPool<GameObject> pool;
    private int currentStage;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(idleBattleEnemyPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: 15,
            maxSize: 25
        );
    }

    void Start()
    {
        currentStage = 1; // get
        StartStage(currentStage);
    }

    private GameObject SetEnemy(ElementalId id, int level)
    {
        Transform spawLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Length)];
        GameObject obj = pool.Get();
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.position = spawLocation.position;

        BattleEnemyPrefab prefabScript = obj.GetComponent<BattleEnemyPrefab>();
        prefabScript.Initialize(id, level, onEnemeyDeath);
        return obj;
    }

    private void StartStage(int waveNum)
    {
        Dictionary<ElementalId, int> stage = StageCatalog.Instance.GetStage(waveNum);
        foreach (KeyValuePair<ElementalId, int> kvp in stage)
        {
            SetEnemy(kvp.Key, kvp.Value);
        }
        enemiesCounter = stage.Count;
    }

    private void onEnemeyDeath(GameObject obj)
    {
        pool.Release(obj);
        enemiesCounter--;
        
        if (enemiesCounter == 0)
        {
            currentStage++;
            StartStage(currentStage);
        }
    }
}

