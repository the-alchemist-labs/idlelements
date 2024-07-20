using System.Collections;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public GameObject playerInfoPanel;

    private static MainManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        StartCoroutine(Temple.StartRoutine());
    }

    public bool IsReady()
    {
        return
            Player.Instance.Id != null 
            && ElementalManager.Instance?.lastEncounterDate.Year != 1
            && Player.Instance.Party != null;
    }
}
