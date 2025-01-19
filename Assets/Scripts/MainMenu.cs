using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // Ensures the game time is running when the main menu scene loads
        Time.timeScale = 1f;

        // Unlocks and shows the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayMainArea()
    {
        PlayerPrefs.SetString("SpawnArea", "MainArea"); // store main spawn area 
        SceneManager.LoadScene("MainGame"); // load the maingame scene
    }

    public void PlayTutorialArea()
    {
        PlayerPrefs.SetString("SpawnArea", "TutorialArea"); // store the tutorial spawn area
        SceneManager.LoadScene("MainGame"); // load the maingame scene
    }

    public void QuitGame()
    {
        Application.Quit(); // quits the game 
    }
}
