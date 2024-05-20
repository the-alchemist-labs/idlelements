using UnityEngine;
using PlayerInterface;

public class Player : MonoBehaviour, IPlayerData
{
    internal static Player instance { get; private set; }

    public PlayerInfo playerInfo { get; set; }
    public Inventory inventory { get; set; }
    public int nextLevelRequiredExp { get; set; }
    public string mapLocation { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // for debug
        if (playerInfo.playerId == null) {
            LoadPlayerData();
        }
    }

    public async void LoadPlayerData()
    {
        string playerId = PlayerPrefs.GetString("playerId");
        PlayerData playerData = await PlayerDataHandler.FetchPlayerData(playerId);
        playerInfo = playerData.playerInfo;
        inventory = playerData.inventory;
        nextLevelRequiredExp = playerData.nextLevelRequiredExp;
        mapLocation = playerData.mapLocation;
        print("ign " + playerInfo.ign);
    }
}
