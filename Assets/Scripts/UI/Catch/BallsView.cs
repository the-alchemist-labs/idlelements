using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BallsScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject ballPrefab;

    public BallId selectedBall;

    void Start()
    {
        GameEvents.OnBallsUpdated += RenderScrollView;
        selectedBall = (BallId)PlayerPrefs.GetInt(PlayerPrefKeys.SELECTED_BALL, (int)BallId.Normal);

        RenderScrollView();
    }

    void OnDestroy()
    {
        GameEvents.OnBallsUpdated -= RenderScrollView;
    }

    void RenderScrollView()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (KeyValuePair<BallId, int> ball in Player.Instance.Inventory.Balls)
        {
            GameObject newBall = Instantiate(ballPrefab, scrollViewContent);
            if (newBall.TryGetComponent(out BallPrefab item))
            {
                item.UpdateBallUI(ball.Key, ball.Value, selectedBall == ball.Key, UpdateSelectedBall);
            }
        }
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void UpdateSelectedBall(BallId ballId)
    {
        selectedBall = ballId;
        RenderScrollView();
        GameEvents.BallSelected();
    }
}
