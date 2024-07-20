using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FriendsScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject friendPrefab;
    public GameObject pendingRequestPrefab;

    void Start()
    {
        GameEvents.OnFriendsUpdated += RenderPendingRequestList;
        RenderPendingRequestList();
    }

    void OnDestroy()
    {
        GameEvents.OnFriendsUpdated -= RenderPendingRequestList;
    }

    void RenderPendingRequestList()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (PlayerInfo request in Player.Instance.Friends.PendingFriendRequestsList)
        {
            GameObject newRequest = Instantiate(pendingRequestPrefab, scrollViewContent);
            if (newRequest.TryGetComponent(out PendingRequestPrefab item))
            {
                item.Init(request);
            }
        }

        foreach (PlayerInfo friend in Player.Instance.Friends.FriendsList)
        {
            GameObject newRequest = Instantiate(friendPrefab, scrollViewContent);
            if (newRequest.TryGetComponent(out FriendPrefab item))
            {
                item.Init(friend);
            }
        }
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
