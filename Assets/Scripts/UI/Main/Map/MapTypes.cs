using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapTypes : MonoBehaviour
{
    public Transform typesContainer;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateMapTypes;
        UpdateMapTypes();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateMapTypes;
    }

    public void UpdateMapTypes()
    {
        typesContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (ElementType typeName in State.Maps.currentMap.mapElementalTypes)
        {
            Sprite newType = Resources.Load<Sprite>($"Sprites/Types/{typeName}");
            GameObject newTypeObject = new GameObject();
            Image spriteRenderer = newTypeObject.AddComponent<Image>();
            spriteRenderer.sprite = newType;
            Instantiate(newTypeObject, typesContainer);
        }
    }
}