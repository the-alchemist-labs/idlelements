using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] panels;

    private int currentTabIndex = 2;
    private int lastTabIndex = 0;

    void Start()
    {
        UpdateTabActive();
    }

    public void OnTabClick(int tabIndex)
    {
        lastTabIndex = currentTabIndex;
        currentTabIndex = tabIndex;
        UpdateTabActive();
    }

    void UpdateTabActive()
    {        
        panels[lastTabIndex].SetActive(false);
        panels[currentTabIndex].SetActive(true);

        ColorBlock lastTabColors = buttons[lastTabIndex].colors;
        lastTabColors.normalColor = Color.yellow;
        buttons[lastTabIndex].colors = lastTabColors;

        ColorBlock currentTabColors = buttons[currentTabIndex].colors;
        currentTabColors.normalColor = Color.green;
        buttons[currentTabIndex].colors = currentTabColors;
    }
}