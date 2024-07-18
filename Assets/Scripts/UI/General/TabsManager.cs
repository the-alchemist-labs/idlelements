using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] panels;

    private int currentTabIndex = 4;
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
    }
}