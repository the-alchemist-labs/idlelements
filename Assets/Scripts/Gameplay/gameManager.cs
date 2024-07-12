using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public GameObject afkGainsPanel;

    private static GameManager instance;

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
        new InitializeUnityServices();

        if (IsFirstTimePlaying())
        {
            SendFirstTimePlayingEvent();
            SetFirstTimePlayingFlag();
        }

        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;

        afkGainsPanel.GetComponent<AfkGainsPanel>()?.DisplayAfkGains();
        StartCoroutine(Temple.StartRoutine());
        StartCoroutine(Backup());
    }

    void OnDestroy()
    {
        State.Save();
    }

    IEnumerator Backup()
    {
        State.Save();
        yield return new WaitForSeconds(1);
    }

    bool IsFirstTimePlaying()
    {
        return !PlayerPrefs.HasKey(PlayerPrefKeys.FirstTimePlaying);
    }

    void SetFirstTimePlayingFlag()
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.FirstTimePlaying, 1);
        PlayerPrefs.Save();
    }

    void SendFirstTimePlayingEvent()
    {
        Analytics.CustomEvent(PlayerPrefKeys.FirstTimePlaying, new Dictionary<string, object>
        {
            { "time", System.DateTime.Now.ToString() }
        });
    }
}
