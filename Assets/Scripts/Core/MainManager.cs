using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public GameObject afkGainsPanel;
    public GameObject playerInfoPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        afkGainsPanel.GetComponent<AfkGainsPanel>()?.DisplayAfkGains();
    }
}
