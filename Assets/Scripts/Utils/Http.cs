using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;


public static class UnityWebRequestAsyncOperationExtension
{
    public static TaskAwaiter<UnityWebRequestAsyncOperation> GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        TaskCompletionSource<UnityWebRequestAsyncOperation> tcs = new TaskCompletionSource<UnityWebRequestAsyncOperation>();
        asyncOp.completed += asyncOperation => tcs.SetResult(asyncOp);
        return tcs.Task.GetAwaiter();
    }
}

public static class Http
{
    public static async Task<T> Get<T>(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"http://{url}"))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return JsonUtility.FromJson<T>(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                return default;
            }
        }
    }

    public static async Task<T> Post<T>(string url, object postData)
    {
        string jsonData = JsonConvert.SerializeObject(postData);
        using (UnityWebRequest request = new UnityWebRequest($"http://{url}", UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                
                return JsonUtility.FromJson<T>(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                return default;
            }
        }
    }
}
