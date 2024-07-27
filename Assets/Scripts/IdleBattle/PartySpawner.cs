using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PartySpawner : MonoBehaviour
{
    private const int DEATH_PENALTY_SECONDS = 5;
    [SerializeField] GameObject idleBattleMemberPrefab;
    [SerializeField] private GameObject[] members = new GameObject[3];
    [SerializeField] private Transform[] spawnLocations = new Transform[3];
    private ObjectPool<GameObject> pool;
    private List<ElementalId> partyIds;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(idleBattleMemberPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: 3,
            maxSize: 5
        );
    }

    void Start()
    {
        GameEvents.OnPartyUpdated += UpdateTeam;
        UpdateTeam();
    }

    void OnDestroy()
    {
        GameEvents.OnPartyUpdated -= UpdateTeam;
    }


    private void UpdateTeam()
    {
        Party party = Player.Instance.Party;
        partyIds = new List<ElementalId> { party.First, party.Second, party.Third };

        for (int i = 0; i < members.Length; i++)
        {
            if (partyIds[i] == ElementalId.None)
            {
                pool.Release(members[i]);
                members[i] = null;
            };

            members[i] = SetPartyMember(partyIds[i], spawnLocations[i]);
        }
    }

    private GameObject SetPartyMember(ElementalId id, Transform transform)
    {
        GameObject obj = pool.Get();
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.position = transform.position;

        BattleMemberPrefab prefabScript = obj.GetComponent<BattleMemberPrefab>();
        prefabScript.Initialize(id, handleFaintedMember);
        return obj;
    }

    private void handleFaintedMember(GameObject obj)
    {
        int partySlot = partyIds.IndexOf(obj.GetComponent<BattleMemberPrefab>().elemental.id);
        pool.Release(obj);
        members[partySlot] = null;
        StartCoroutine(SpawnNewMemberAfterDelay(partySlot));
    }

    IEnumerator SpawnNewMemberAfterDelay(int partySlot)
    {
        yield return new WaitForSeconds(DEATH_PENALTY_SECONDS);
        if (partyIds[partySlot] != ElementalId.None)
        {
            members[partySlot] = SetPartyMember(partyIds[partySlot], spawnLocations[partySlot]);
        }
    }
}
