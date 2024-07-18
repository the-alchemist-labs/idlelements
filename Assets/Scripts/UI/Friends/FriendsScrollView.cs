using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FriendsScrollView  : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject rowPrefab;

    void Start()
    {
        GameEvents.OnPlayerInitilized += RenderPendingRequestList;
        GameEvents.OnFriendsUpdated += RenderPendingRequestList;
    }

    void OnDestroy()
    {
        GameEvents.OnPlayerInitilized -= RenderPendingRequestList;
        GameEvents.OnFriendsUpdated -= RenderPendingRequestList;
    }

    void RenderPendingRequestList()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        if (Player.Instance.Friends == null) return;
        foreach (PlayerInfo friend in Player.Instance.Friends)
        {
            GameObject newRequest = Instantiate(rowPrefab, scrollViewContent);
            if (newRequest.TryGetComponent(out FriendPrefab item))
            {
                item.Init(friend);
            }
        }
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
