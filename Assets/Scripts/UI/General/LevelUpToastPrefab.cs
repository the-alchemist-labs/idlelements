using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpToastPrefab : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    TMP_Text levelText;
    [SerializeField]
    TMP_Text orbsText;
    [SerializeField]
    GameObject mapContainer;
    [SerializeField]
    TMP_Text mapText;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(CheckIfAnimationFinished());
    }

    public void DisplayToast()
    {
        levelText.text = $"You are level {Player.Instance.Level}!";
        orbsText.text = $"{Consts.LevelUpOrbsGain}";
        Map unlockedMap = MapCatalog.Instance.GetUnlockedMapByLevel(Player.Instance.Level);
        mapContainer.gameObject.SetActive(unlockedMap != null);
        mapText.text = $"{unlockedMap?.name} unlocked";

        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Celebration);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.speed = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _animator.speed = 1;
    }

    private IEnumerator CheckIfAnimationFinished()
    {
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length == 0)
        {
            Debug.LogError("No animation clip found");
            yield break;
        }

        string animationName = clipInfo[0].clip.name;
        while (true)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) ||
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }
}
