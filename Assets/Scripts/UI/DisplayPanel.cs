using UnityEngine;

public class DisplayPanel : MonoBehaviour
{
    public GameObject panel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == gameObject)
        {
            panel.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == gameObject)
        {
            panel.SetActive(false);
        }
    }
}
