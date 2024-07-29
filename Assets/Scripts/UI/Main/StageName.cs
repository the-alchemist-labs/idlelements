using TMPro;
using UnityEngine;

public class StageName : MonoBehaviour
{
    public TMP_Text stageNameText;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateMapName;
        UpdateMapName();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateMapName;
    }

    public void UpdateMapName()
    {
        stageNameText.text = $"Stage {IdleBattleManager.Instance.CurrentStage}";
    }
}