using System.Collections;
using UnityEngine;

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




}
