using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
    public float gameDuration = 180f; // 3 minutes in seconds
    public Text timerText; // Text component to display the timer
    public GameObject gameOverPopup;
    public Text resultText; // Text component to show "You Win!" or "You Lose!"
    public Text winnerText; // Text component to display the winner's name

    private float timer;
    private bool gameEnded = false;

    void Start()
    {
        PhotonNetwork.Instantiate("Multiplayer Player", Vector3.zero, Quaternion.identity);
        timer = gameDuration;
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while (timer > 0 && !gameEnded)
        {
            // Update timer display
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }

        // Time is up, end the game if not already ended
        if (!gameEnded)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;
        Photon.Realtime.Player highestScorer = GetHighestScorer();

        if (highestScorer != null)
        {
            winnerText.text = "Winner: " + highestScorer.NickName;

            // Check if the local player is the winner
            if (PhotonNetwork.LocalPlayer == highestScorer)
            {
                resultText.text = "You Win!";
            }
            else
            {
                resultText.text = "You Lose.";
            }
        }
        else
        {
            // If there's no clear winner (e.g., all scores are 0)
            winnerText.text = "No Winner";
            resultText.text = "Game Over";
        }

        gameOverPopup.SetActive(true);
    }

    Photon.Realtime.Player GetHighestScorer()
    {
        Photon.Realtime.Player highestScorer = null;
        int highestScore = 0;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            int playerScore = player.GetScore();
            if (playerScore > highestScore)
            {
                highestScore = playerScore;
                highestScorer = player;
            }
        }

        return highestScorer;
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("GameScene_Multiplayer");
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Ensure this method only updates the UI and does not end the game
        // Add logic here to update the UI scoreboard if needed
    }
}
