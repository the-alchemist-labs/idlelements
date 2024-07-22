using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorStartScene
{
    private const string startScene = "Assets/Scenes/LoadingScene.unity";
    private const string previousSceneKey = "EditorStartScene_PreviousScene";

    static EditorStartScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string currentScene = EditorSceneManager.GetActiveScene().path;
            EditorPrefs.SetString(previousSceneKey, currentScene);

            if (currentScene != startScene)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(startScene);
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            string previousScene = EditorPrefs.GetString(previousSceneKey, string.Empty);
            if (!string.IsNullOrEmpty(previousScene) && previousScene != startScene)
            {
                EditorApplication.delayCall += () => EditorSceneManager.OpenScene(previousScene);
            }
        }
    }
}
