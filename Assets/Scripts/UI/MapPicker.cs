using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapPicker : MonoBehaviour
{
    public string mapName;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // not final solution since it dont know what gameobject was clicked
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                SceneManager.LoadScene("IdleMapScene");
            }
        }
    }
}
