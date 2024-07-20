using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CatchToastPrefab : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text nameText;
    public TMP_Text tokensText;
    public Image elementalImage;
    public Image newIndicatorImage;
    public TMP_Text expText;
    public TMP_Text orbsText;
    public GameObject orbsPanel;
    public AudioSource newCatchSound;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckIfAnimationFinished());
    }

    public void DisplayToast(Elemental elemental)
    {
        int tokens = ElementalManager.Instance.GetElementalEntry(elemental.id).tokens;
        bool isNew = !ElementalManager.Instance.IsElementalRegistered(elemental.id);

        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        nameText.text = $"You caught {elemental.name}!";
        tokensText.text = $"Tokens: {tokens}";
        newIndicatorImage.gameObject.SetActive(isNew);
        expText.text = TextUtil.NumberFormatter(elemental.expGain);

        if (isNew)
        {
            SoundManager.Instance.PlaySFXFromPrefab(newCatchSound);
            orbsText.text = TextUtil.NumberFormatter(elemental.orbsGain);
        }
        else
        {
            orbsPanel.SetActive(false);
        }
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
