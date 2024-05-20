using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    internal static TabManager Instance { get; private set; }
    public Button[] tabsButtons;
    private int currentTabInex;
    private string[] scences = new string[] {
        "ShopScene",
        "BattleTowerScene",
        "MainScene",
        "MapScene",
        "GuildScene"
        };


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentTabInex = 2; // default
        UpdateTabActive();
    }

    public void OnTabClick(int tabIndex)
    {
        currentTabInex = tabIndex;
        SceneManager.LoadScene(scences[currentTabInex]);
        UpdateTabActive();
    }

    void UpdateTabActive()
    {
        for (int i = 0; i < tabsButtons.Length; i++)
        {
            ColorBlock colors = tabsButtons[i].colors;
            if (i == currentTabInex)
            {
                colors.normalColor = Color.green;
            }
            else
            {
                colors.normalColor = Color.yellow;
            }
            tabsButtons[i].colors = colors;
        }
    }

}