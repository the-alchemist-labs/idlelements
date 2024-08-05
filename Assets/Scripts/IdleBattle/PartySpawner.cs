using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class PartySpawner : MonoBehaviour
{
    public event Action OnPartyWiped;

    [SerializeField] private GameObject _idleBattleMemberPrefab;
    [SerializeField] private Transform[] _spawnLocations = new Transform[3];

    private GameObject[] _members = new GameObject[3];
    private ObjectPool<GameObject> _pool;
    private List<ElementalId> _partyIds;

    void Awake()
    {
        GameEvents.OnPartyUpdated += UpdateTeam;
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_idleBattleMemberPrefab),
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

    public void ResetStage()
    {
        _members.ToList().Clear();
        UpdateTeam();
    }

    private void UpdateTeam()
    {
        Party party = Player.Instance.Party;
        _partyIds = new List<ElementalId> { party.First, party.Second, party.Third };

        for (int i = 0; i < _members.Length; i++)
        {
            if (_partyIds[i] == ElementalId.None)
            {
                if (_members[i] != null)
                {
                    _pool.Release(_members[i]);
                    _members[i] = null;
                }
                continue;
            };

            _members[i] = SetPartyMember(_partyIds[i], _spawnLocations[i]);
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
        GameObject obj = _pool.Get();
        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.position = transform.position;

        BattleMemberPrefab prefabScript = obj.GetComponent<BattleMemberPrefab>();
        prefabScript.InitializeMember(id, Player.Instance.Level);
        return obj;
    }

    private void handleFaintedMember(GameObject obj)
    {
        int partySlot = _partyIds.IndexOf(((Elemental)obj.GetComponent<BattleMemberPrefab>().Elemental).Id);
        _pool.Release(obj);
        _members[partySlot] = null;

        if (_members.All(item => item == null))
        {
            OnPartyWiped?.Invoke();
        }
    }
}
