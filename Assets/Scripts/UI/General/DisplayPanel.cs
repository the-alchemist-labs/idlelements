using UnityEngine;

public class DisplayPanel : MonoBehaviour
{
    public GameObject panel;

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
