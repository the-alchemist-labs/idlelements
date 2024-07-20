using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadingSlider : MonoBehaviour
{
    public Slider loadingSlider;
    private float fillSpeed = 2.3f;

    void Start()
    {
        loadingSlider.value = 0;
        StartCoroutine(UpdateSlider());
    }

    IEnumerator UpdateSlider()
    {
        while (loadingSlider.value < 1f)
        {
            loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, 1, fillSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
