using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public int enemies = 5; // Initial number of enemies
    public Text enemiesText; // UI Text to display remaining enemies
    public GameObject winScreen; // Win screen UI element

    private void Awake()
    {
        // Ensure enemiesText and winScreen are assigned
        if (enemiesText == null)
        {
            Debug.LogError("EnemiesText is not assigned in the Inspector.");
            return;
        }
        if (winScreen == null)
        {
            Debug.LogError("WinScreen is not assigned in the Inspector.");
            return;
        }

        // Initialize the text display
        enemiesText.text = enemies.ToString();

        // Subscribe to the enemy killed event
        Enemy.OnEnemyKilled += OnEnemyKilledAction;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks or errors when the script is destroyed
        Enemy.OnEnemyKilled -= OnEnemyKilledAction;
    }

    // Method called when an enemy is killed
    private void OnEnemyKilledAction()
    {
        enemies--;

        // Update the UI text
        if (enemiesText != null)
        {
            enemiesText.text = enemies.ToString();
        }

        // Check if all enemies are defeated
        if (enemies <= 0)
        {
            winScreen.SetActive(true);
        }
    }
}
