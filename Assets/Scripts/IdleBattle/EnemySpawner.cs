using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject idleBattleEnemyPrefab;
    [SerializeField] private Transform[] spawnLocations = new Transform[3];

    void Start()
    {
        GameObject enemy = Instantiate(idleBattleEnemyPrefab, spawnLocations[0].position, Quaternion.identity, transform);
        enemy.GetComponent<BattleEnemyPrefab>().SetEnemy(ElementalId.Ferine);
         GameObject enemy2 = Instantiate(idleBattleEnemyPrefab, spawnLocations[1].position, Quaternion.identity, transform);
        enemy2.GetComponent<BattleEnemyPrefab>().SetEnemy(ElementalId.Ferine);
         GameObject enemy3 = Instantiate(idleBattleEnemyPrefab, spawnLocations[2].position, Quaternion.identity, transform);
        enemy3.GetComponent<BattleEnemyPrefab>().SetEnemy(ElementalId.Ferine);
    }
}

