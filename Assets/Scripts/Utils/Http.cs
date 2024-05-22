using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


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
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
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