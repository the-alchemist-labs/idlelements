using UnityEngine;

public class DynnamicScrollView : MonoBehaviour
{
    public Transform scrollViewContent;
    public GameObject rowPrefab;

    void Start()
    {
        foreach (Elemental entry in Elementals.all)
        {
            GameObject newEntry = Instantiate(rowPrefab, scrollViewContent);
            if (newEntry.TryGetComponent(out scrollViewDeckEntry item))
            {
                item.UpdateEntry(entry);
            }
        }
    }

}
