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
        GameEvents.OnPartyUpdated += UpdateTeam;
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(idleBattleMemberPrefab),
            actionOnGet: GetInstance,
            actionOnRelease: ReleaseInstance,
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: 3,
            maxSize: 5
        );
    }

    void Start()
    {
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
                if (members[i] != null)
                {
                    pool.Release(members[i]);
                    members[i] = null;
                }
                continue;
            };

            members[i] = SetPartyMember(partyIds[i], spawnLocations[i]);
        }
    }

    private void GetInstance(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<BattleMemberPrefab>().OnDefeat += handleFaintedMember;
    }

    private void ReleaseInstance(GameObject obj)
    {
        obj.GetComponent<BattleMemberPrefab>().OnDefeat -= handleFaintedMember;
        obj.SetActive(false);
    }

    private GameObject SetPartyMember(ElementalId id, Transform transform)
    {
        GameObject obj = pool.Get();
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.position = transform.position;

        BattleMemberPrefab prefabScript = obj.GetComponent<BattleMemberPrefab>();
        prefabScript.InitializeMember(id, Player.Instance.Level);
        return obj;
    }

    private void handleFaintedMember(GameObject obj)
    {
        int partySlot = partyIds.IndexOf(((Elemental)obj.GetComponent<BattleMemberPrefab>().elemental).id);
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
