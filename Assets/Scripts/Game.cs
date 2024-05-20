using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("playerId", "12345");
        PlayerPrefs.Save();


        if (string.IsNullOrEmpty(PlayerPrefs.GetString("playerId")))
        {
            Debug.Log("No player ID found. Returning to login scene.");
            return;
        }

        if (Player.instance == null)
        {
            GameObject playerObject = new GameObject("Player");
            playerObject.AddComponent<Player>();
        }

        Player.instance.LoadPlayerData();
        SceneManager.LoadScene("mainScene");

    }
}
