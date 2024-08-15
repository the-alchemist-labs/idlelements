using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DailiesScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject dailyPrefab;

    void Start()
    {
        GameEvents.OnDailyUpdated += RenderList;
        RenderList();
    }

    void OnDestroy()
    {
        GameEvents.OnDailyUpdated -= RenderList;
    }

    void RenderList()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        
        foreach (DailyProgress daily in DailiesManager.Instance.GetDailies())
        {
            GameObject newDaily = Instantiate(dailyPrefab, scrollViewContent);
            if (newDaily.TryGetComponent(out DailyPrefab item))
            {
                item.Init(daily);
            }
        }
        scrollRect.verticalNormalizedPosition = 1f;
    }
}