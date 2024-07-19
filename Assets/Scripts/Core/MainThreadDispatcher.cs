using System;
using System.Threading;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static SynchronizationContext mainThreadContext;

    void Awake()
    {
        mainThreadContext = SynchronizationContext.Current;
    }

    public static void Enqueue(Action action)
    {
        if (mainThreadContext == null)
        {
            throw new InvalidOperationException("MainThreadDispatcher not initialized. Ensure it's added to a GameObject in your scene.");
        }
        mainThreadContext.Post(_ => action(), null);
    }
}
