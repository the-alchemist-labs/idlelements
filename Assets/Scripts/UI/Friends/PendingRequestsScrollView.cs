using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PendingRequestsScrollView : MonoBehaviour
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
        bool shouldDisplay = Player.Instance.PendingFriendRequests?.Count  > 0;

        gameObject.SetActive(shouldDisplay);

        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        if (Player.Instance.PendingFriendRequests == null) return;
        foreach (PlayerInfo request in Player.Instance.PendingFriendRequests)
        {
            GameObject newRequest = Instantiate(rowPrefab, scrollViewContent);
            if (newRequest.TryGetComponent(out PendingRequestPrefab item))
            {
                item.Init(request);
            }
        }
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
