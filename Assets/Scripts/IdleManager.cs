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
            SceneManager.sceneLoaded += OnSceneLoaded;
            StartCoroutine(SaveTimestampRoutine());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (scene.name == SceneNames.IdleMap)
        {
            Debug.Log("calc");
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
