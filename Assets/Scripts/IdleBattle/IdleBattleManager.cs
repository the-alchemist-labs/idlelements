using System.Collections.Generic;
using UnityEngine;

public class IdleBattleManager : MonoBehaviour
{
    public static IdleBattleManager Instance;

    public GameObject prefab;
    private const int EFFECT_POOL_SIZE = 30;
    private List<GameObject> effectPool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    private void InitializeEffectPool()
    {
        for (int i = 0; i < EFFECT_POOL_SIZE; i++)
        {
        }
    }
}
