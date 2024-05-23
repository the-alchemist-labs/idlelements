using UnityEngine;

public class Player : MonoBehaviour
{
    internal static Player instance { get; private set; }

    public PlayerInfo playerInfo;

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

    void Update()
    {
        print(PlayerPrefs.GetString("Gold"));
        HeaderBannerManager.Instance.UpdateGoldText($"{PlayerPrefs.GetInt("Gold")}");
    }

    public async void LoadPlayerData()
    {
        string playerId = PlayerPrefs.GetString("playerId");
        PlayerData playerData = await PlayerApi.GetData<PlayerData>(playerId);
        playerInfo = playerData.playerInfo;
    }
}
