using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BallsScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject ballPrefab;

    private List<GameObject> ballGameObjects;

    void Start()
    {
        ballGameObjects = new List<GameObject>();

        GameEvents.OnBallsUpdated += RenderScrollView;
        GameEvents.OnBallSelected += RenderScrollView;

        RenderScrollView();
    }

    void OnDestroy()
    {
        GameEvents.OnBallsUpdated -= RenderScrollView;
        GameEvents.OnBallSelected -= RenderScrollView;
    }

    void RenderScrollView()
    {
        ClearBalls();

        foreach (KeyValuePair<BallId, int> ball in Player.Instance.Inventory.Balls)
        {
            GameObject newBall = Instantiate(ballPrefab, scrollViewContent);
            if (newBall.TryGetComponent(out BallPrefab item))
            {
                item.UpdateBallUI(ball.Key, ball.Value);
                item.OnBallSelected += UpdateSelectedBall;
                ballGameObjects.Add(newBall);
            }
        }
    }

    private void UpdateSelectedBall(BallId ballId)
    {
        EncounterManger.Instance.UpdateSelectedBall(ballId);
    }

    private void ClearBalls()
    {
        foreach (GameObject ball in ballGameObjects)
        {
            if (ball != null && ball.TryGetComponent(out BallPrefab item))
            {
                item.OnBallSelected -= UpdateSelectedBall;
            }
        }
        
        ballGameObjects.Clear();
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
    }
}