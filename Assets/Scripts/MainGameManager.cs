using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public Transform mainAreaSpawn;     // Reference to the Main Area spawn point
    public Transform tutorialAreaSpawn; // Reference to the Tutorial Area spawn point
    public GameObject player;           // Reference to the player object
    public PlayerMovement playerMovementScript; // Reference to the PlayerMovement script

    private bool isGamePaused = false;  // Flag to check if the game is paused

    void Start()
    {
        // Ensure player movement is enabled when the game starts
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;  // Enable movement if not already enabled
        }

        // Retrieve spawn area from PlayerPrefs (defaults to "MainArea" if not set)
        string spawnArea = PlayerPrefs.GetString("SpawnArea", "MainArea");

        // Determine the spawn area and set the player position and rotation accordingly
        if (spawnArea == "MainArea" && mainAreaSpawn != null)
        {
            player.transform.position = mainAreaSpawn.position;
            player.transform.rotation = mainAreaSpawn.rotation;
        }
        else if (spawnArea == "TutorialArea" && tutorialAreaSpawn != null)
        {
            player.transform.position = tutorialAreaSpawn.position;
            player.transform.rotation = tutorialAreaSpawn.rotation;
        }
    }

    void Update()
    {
        // Check for Escape key press to pause or go to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                GoToMainMenu();
            }
        }
    }

    // Function to resume the game (unpause)
    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resumes time in the game
        isGamePaused = false; // Set the paused state to false

        // Enable player movement
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }

    // Function to pause the game and go to the main menu
    private void GoToMainMenu()
    {
        // Pause the game by stopping player movement and freezing time
        Time.timeScale = 0f;
        isGamePaused = true; // Set the paused state to true

        // Disable player movement
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // Set PlayerPrefs to default spawn area (MainArea)
        PlayerPrefs.SetString("SpawnArea", "MainArea");

        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }

    // This method will be called when the game is resumed from the Main Menu
    public void StartNewGame(string spawnArea)
    {
        // Set the spawn area to either MainArea or TutorialArea
        PlayerPrefs.SetString("SpawnArea", spawnArea);

        // Load the Main Game scene
        SceneManager.LoadScene("MainGame");
    }
}
