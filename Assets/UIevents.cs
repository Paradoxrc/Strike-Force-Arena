using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Include this for scene management

public class UIevents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Method to restart the current level
    public void RestartLevel()
    {
        // Reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to go back to the main menu (Scene 2)
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
