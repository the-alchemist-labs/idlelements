using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckPopup : BasePopup
{
    public override PopupId Id { get; } = PopupId.Deck;

    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject rowPrefab;

    void Awake()
    {
        SetupCloseableBackground(true);
    }
    
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

        foreach (Elemental entry in ElementalCatalog.Instance.Elementals)
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
