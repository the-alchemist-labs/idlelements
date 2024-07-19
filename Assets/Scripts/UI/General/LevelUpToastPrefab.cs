using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpToastPrefab : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text levelText;
    public TMP_Text orbsText;

    public GameObject mapContainer;
    public TMP_Text mapText;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckIfAnimationFinished());
    }

    public void DisplayToast()
    {
        levelText.text = $"You are level {Player.Instance.Level}!";
        orbsText.text = $"+{Consts.LevelUpOrbsGain}";
        Map unlockedMap = MapsData.Instance.GetUnlockedMapByLevel(Player.Instance.Level);
        mapContainer.gameObject.SetActive(unlockedMap != null);
        mapText.text = $"{unlockedMap.name} unlocked";
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private IEnumerator CheckIfAnimationFinished()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length == 0)
        {
            Debug.LogError("No animation clip found");
            yield break;
        }

        string animationName = clipInfo[0].clip.name;
        while (true)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) ||
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.speed = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.speed = 1;
    }
}
