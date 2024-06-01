using UnityEngine;
using UnityEngine.UI;

public class DynnamicScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject rowPrefab;

    void Start()
    {
        foreach (Elemental entry in State.Elementals.all)
        {
            GameObject newEntry = Instantiate(rowPrefab, scrollViewContent);
            if (newEntry.TryGetComponent(out scrollViewDeckEntry item))
            {
                item.UpdateEntry(entry);
            }
        }

        // Scroll to top of the scrollview
        scrollRect.verticalNormalizedPosition = 1f;
    }

}
