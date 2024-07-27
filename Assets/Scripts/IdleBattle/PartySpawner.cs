using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    [SerializeField] GameObject idleBattleMemberPrefab;
    [SerializeField] private GameObject[] members = new GameObject[3];
    [SerializeField] private Transform[] spawnLocations = new Transform[3];

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
        ElementalId[] partyIds = { party.First, party.Second, party.Third };

        for (int i = 0; i < members.Length; i++)
        {
            Destroy(members[i]);
            if (partyIds[i] == ElementalId.None) return;

            members[i] = Instantiate(idleBattleMemberPrefab, spawnLocations[i].position, Quaternion.identity, transform);
            members[i].GetComponent<BattleMemberPrefab>().SetElemental(partyIds[i]);
        }
    }
}
