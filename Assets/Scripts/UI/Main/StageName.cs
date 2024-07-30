using TMPro;
using UnityEngine;

public class StageName : MonoBehaviour
{
    public TMP_Text stageNameText;

    void Start()
    {
        GameEvents.OnStageFinished += UpdateStageName;
        UpdateStageName();
    }

    void OnDestroy()
    {
        GameEvents.OnStageFinished -= UpdateStageName;
    }

    public void UpdateStageName()
    {
        stageNameText.text = $"Stage {IdleBattleManager.Instance.GetStageName()}";
    }
}