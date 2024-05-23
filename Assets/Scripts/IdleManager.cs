using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleManager : MonoBehaviour
{
    private static IdleManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(SaveTimestampRoutine());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SaveTimestampRoutine()
    {
        while (true)
        {
            int currentGold = PlayerPrefs.GetInt("Gold", 0);
            currentGold++;
            PlayerPrefs.SetInt("Gold", currentGold);
            PlayerPrefs.SetString("LastTimestamp", System.DateTime.Now.ToString());
            PlayerPrefs.Save();
            yield return new WaitForSeconds(1);
        }
    }
}
