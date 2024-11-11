using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public int enemies = 5;
    public Text enemiesText;
    public GameObject winScreen;


    private void Awake()
    {
        enemiesText.text = enemies.ToString();
        Enemy.OnEnemyKilled += OnEnemyKilledAction;
    }

    void OnEnemyKilledAction()
    {
        enemies--;
        enemiesText.text = enemies.ToString();
    }

    void Update()
    {
        if (enemies == 0)
        {
            winScreen.SetActive(true);
        }

    }

}
