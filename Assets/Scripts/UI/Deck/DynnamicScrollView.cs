using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DynnamicScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject rowPrefab;

    void Start()
    {
        GameEvents.OnElementalCaught += PopulateDeck;
        GameEvents.OnEssenceUpdated += PopulateDeck;
        GameEvents.OnTokensUpdated += PopulateDeck;
        PopulateDeck();
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= PopulateDeck;
        GameEvents.OnEssenceUpdated -= PopulateDeck;
        GameEvents.OnTokensUpdated -= PopulateDeck;
    }

    void PopulateDeck()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (Elemental entry in State.Elementals.all)
        {
            GameObject newEntry = Instantiate(rowPrefab, scrollViewContent);
            if (newEntry.TryGetComponent(out DeckEntry item))
            {
                item.UpdateEntry(entry);
            }
        }

        // Scroll to top of the scrollview
        scrollRect.verticalNormalizedPosition = 1f;
    }
}