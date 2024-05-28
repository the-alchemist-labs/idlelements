using TMPro;
using UnityEngine;

public class HeaderBannerManager : MonoBehaviour
{
    public TMP_Text essenseText;
    public TMP_Text orbsText;
    public TMP_Text levelText;
    public TMP_Text expText;

    private static HeaderBannerManager instance;
    public static HeaderBannerManager Instance { get { return instance; } }

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

    void Update()
    {
        essenseText.text = $"Essence: {State.essence}";
        orbsText.text = $"Orbs: {State.orbs}";
        levelText.text = $"Level: {State.level}";
        expText.text = $"Exp: {State.experience}/{State.requiredExpToLevelUp[State.level]}";
    }
}