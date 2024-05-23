using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadIdleMapScene()
    {
        SceneManager.LoadScene(SceneNames.IdleMap);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(SceneNames.Main);
    }
}