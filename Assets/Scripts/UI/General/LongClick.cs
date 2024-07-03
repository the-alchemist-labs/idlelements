using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onClick;
    public float interval;

    public void OnPointerDown(PointerEventData eventData)
    {
        InvokeRepeating("InvokeAction", 0, interval);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("InvokeAction");
    }

    private void InvokeAction()
    {
        onClick?.Invoke();
    }
}
