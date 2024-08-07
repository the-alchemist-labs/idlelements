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
    public MainSceneTab ActiveTab = MainSceneTab.Main;

    [SerializeField] Button[] buttons;
    [SerializeField] GameObject[] panels;

    private MainSceneTab _lastTabIndex = 0;

    void Start()
    {
        UpdateTabActive();
    }

    public void OnTabClick(int tabIndex)
    {
        _lastTabIndex = ActiveTab;
        ActiveTab = (MainSceneTab)tabIndex;
        UpdateTabActive();
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
    }

    void UpdateTabActive()
    {        
        panels[(int)_lastTabIndex].SetActive(false);
        panels[(int)ActiveTab].SetActive(true);
    }
}