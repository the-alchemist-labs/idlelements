using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image rewardImage;
    [SerializeField] private TMP_Text rewardAmountText;
    [SerializeField] private GameObject claimed;
    
    public void Init()
    {
        
    }

    public void Claim()
    {
        //
    }
}
