using UnityEngine;
using UnityEngine.UI;

public enum MainSceneTab
{
    Shop,
    Encounter,
    Main,
    Tower,
    Guild
}

public class TabManager : MonoBehaviour
{
    public MainSceneTab ActiveTab;

    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] panels;

    private MainSceneTab _lastTabIndex = 0;

    void Start()
    {   
        ActiveTab = MainSceneTab.Main;
        UpdateActiveTab();
    }

    public void OnTabClick(int tabIndex)
    {
        _lastTabIndex = ActiveTab;
        ActiveTab = (MainSceneTab)tabIndex;
        UpdateActiveTab();
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
    }

    void UpdateActiveTab()
    {        
        panels[(int)_lastTabIndex].SetActive(false);
        panels[(int)ActiveTab].SetActive(true);
    }
}