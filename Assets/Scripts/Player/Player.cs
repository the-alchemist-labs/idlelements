using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal static Player instance { get; private set; }

    public TMP_Text  textField;
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
        if (textField != null)
        {   
            string gold = PlayerPrefs.GetInt("Gold").ToString();
            textField.text = "Score: " + gold;
        }
    }

    public async void LoadPlayerData()
    {
        string playerId = PlayerPrefs.GetString("playerId");
        PlayerData playerData = await PlayerApi.GetData<PlayerData>(playerId);
        playerInfo = playerData.playerInfo;
    }
}
