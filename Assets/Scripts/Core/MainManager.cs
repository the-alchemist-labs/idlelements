using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public GameObject AfkGainsPanel;
    public GameObject PlayerInfoPanel;
    public GameObject SelectPartyMemberPanel;
    public GameObject SelectSkillPanel;
    public TabManager TabManager;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            gameObject.AddComponent<NotificationManager>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AfkGainsPanel.GetComponent<AfkGainsPanel>()?.DisplayAfkGains();
    }
}
