using TMPro;
using UnityEngine;

public class HeaderBannerManager : MonoBehaviour
{
    public TMP_Text goldText;

    private static HeaderBannerManager instance;
    public static HeaderBannerManager Instance{get { return instance; }}
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdateGoldText(string value)
    {
        goldText.text = $"Gold: {value}";
    }
}