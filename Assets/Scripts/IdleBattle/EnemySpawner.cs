using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject idleBattleEnemyPrefab;
    [SerializeField] private Transform[] spawnLocations = new Transform[5];
    [SerializeField] private int enemiesCounter;
    private ObjectPool<GameObject> pool;
    private int currentStage;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(idleBattleEnemyPrefab),
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
        currentStage = 1; // get
        StartStage(currentStage);
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

    private GameObject SetEnemy(ElementalId id, int level)
    {
        Transform spawLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
        GameObject obj = pool.Get();
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.position = spawLocation.position;

        BattleEnemyPrefab prefabScript = obj.GetComponent<BattleEnemyPrefab>();
        prefabScript.Initialize(id, level);
        return obj;
    }

    private void StartStage(int waveNum)
    {
        List<StageMinimemtal> stage = StageCatalog.Instance.GetStage(waveNum);
        foreach (StageMinimemtal stageMinimemtal in stage)
        {
            SetEnemy(stageMinimemtal.Id, stageMinimemtal.Level);
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

