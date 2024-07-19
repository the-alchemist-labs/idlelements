using UnityEngine;

public class ResourcesData : MonoBehaviour
{
    public static ResourcesData Instance { get; private set; }

    public int Essence { get; private set; }
    public int Gold { get; private set; }
    public int Orbs { get; private set; }

    private void Awake()
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
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State, true);
        Essence = gs.essence;
        Gold = gs.gold;
        Orbs = gs.orbs;
    }

    public void UpdateEssence(int amount)
    {
        Essence = (Essence + amount >= 0) ? Essence + amount : 0;
        GameEvents.EssenceUpdated();
    }

    public void UpdateGold(int amount)
    {
        Gold = (Gold + amount >= 0) ? Gold + amount : 0;
        GameEvents.GoldUpdated();
    }

    public void UpdateOrbs(int amount)
    {
        Orbs = (Orbs + amount >= 0) ? Orbs + amount : 0;
    }
}