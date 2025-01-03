using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallPrefab : MonoBehaviour
{
    public event Action<BallId> OnBallSelected;
    public Image ballImage;
    public TMP_Text ballsAmountText;
    public GameObject selectedIndicator;

    private BallId ballId;
    private int ballsAmount = 0;

    public void UpdateBallUI(BallId ballId, int ballsAmount)
    {
        this.ballId = ballId;
        this. ballsAmount = ballsAmount;
        bool isSelected = ballId == EncounterManger.Instance.SelectedBallId;
        
        ballImage.sprite = Resources.Load<Sprite>($"Sprites/Inventory/Balls/{ballId}");
        ballImage.color = ballsAmount > 0 ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        ballsAmountText.text = $"X{ballsAmount}";
        selectedIndicator.SetActive(isSelected);
    }

    public void BallSelected()
    {
        if (ballsAmount > 0)
        {
            OnBallSelected?.Invoke(ballId);
        }
    }
}