using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; 
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that touches the platform has a player tag.
        if (other.CompareTag("Player"))
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
