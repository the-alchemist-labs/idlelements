using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public static class PlayerApi
{
    public static async Task<PlayerData> GetData<PlayerData>(string playerId)
    {    
        string url = $"http://localhost:3000/player/{playerId}";
        TaskCompletionSource<PlayerData> tcs = new TaskCompletionSource<PlayerData>();
        CoroutineRunner.Instance.StartCoroutine(GetCoroutine(url, tcs));
        return await tcs.Task;
    }

    private static IEnumerator GetCoroutine<T>(string url, TaskCompletionSource<T> tcs)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                T playerData = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                tcs.SetResult(playerData);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
                tcs.SetException(new System.Exception(webRequest.error));
            }
        }
    }
}

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;
    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
}
