using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public GameObject playerInfoPanel;

    private static MainManager instance;

    void Awake()
    {
        if (Player.Instance == null)
        {
            SceneManager.LoadScene(SceneNames.Loading);
            return;
        }

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
}
