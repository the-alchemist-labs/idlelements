using UnityEngine;

public class DisplayPanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject backgroundPanel;

    public void OpenPanel()
    {
        
        backgroundPanel?.SetActive(true);
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        backgroundPanel?.SetActive(false);
        panel.SetActive(false);
    }
}
