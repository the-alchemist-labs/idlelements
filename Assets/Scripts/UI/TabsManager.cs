using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] tabsButtons;
    public int currentTabInex;

    private string[] scences = new string[] {
        "ShopScene",
        "BattleTowerScene",
        "MainScene",
        "MapScene",
        "GuildScene"
        };

    void Start()
    {
        UpdateTabActive();
    }

    public void OnTabClick(int tabIndex)
    {
        SceneManager.LoadScene(scences[tabIndex]);
        UpdateTabActive();
    }

    void UpdateTabActive()
    {
        for (int i = 0; i < tabsButtons.Length; i++)
        {
            ColorBlock colors = tabsButtons[i].colors;
            if (i == currentTabInex)
            {
                colors.normalColor = Color.green; // Mark the current tab
            }
            else
            {
                colors.normalColor = Color.white;
            }
            tabsButtons[i].colors = colors;
        }
    }
}