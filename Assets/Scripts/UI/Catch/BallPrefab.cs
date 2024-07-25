using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallPrefab : MonoBehaviour
{
    public Image ballImage;
    public TMP_Text ballsAmountText;
    public GameObject selectedIndicator;

    private BallId ballId;
    private int ballsAmount = 0;
    private System.Action<BallId> updateSelectedBall;

    public void UpdateBallUI(BallId ballId, int ballsAmount, bool isSelected, System.Action<BallId> updateSelectedBall)
    {
        this.ballId = ballId;
        this.updateSelectedBall = updateSelectedBall;
        this. ballsAmount = ballsAmount;
        ballImage.sprite = Resources.Load<Sprite>($"Sprites/Inventory/Balls/{ballId}");
        ballImage.color = ballsAmount > 0 ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        ballsAmountText.text = $"X{ballsAmount}";
        selectedIndicator.SetActive(isSelected);
    }

    public void BallSelected()
    {
        if (ballsAmount > 0)
        {
            updateSelectedBall(ballId);
        }
    }
}