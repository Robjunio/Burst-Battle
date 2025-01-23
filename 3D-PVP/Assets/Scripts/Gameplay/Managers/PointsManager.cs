using System.Collections;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private GameObject pointsScreem;
    [SerializeField] TMP_Text[] playerUI;

    private int PointsMax = 1;
    private int[] players = new int[4];

    private void PlayerGotPoint(string player)
    {
        print("Somebody Die " + player);
        switch (player)
        {
            case "Player 1":
                players[0] = players[0] + 1;
                break;
            case "Player 2":
                players[1] = players[1] + 1;
                break;
            case "Player 3":
                players[2] = players[2] + 1;
                break;
            case "Player 4":
                players[3] = players[3] + 1;
                break;
        }
    }

    private void PlayerLostPoint(string player)
    {
        switch (player)
        {
            case "Player 1":
                players[0] = Mathf.Clamp(players[0] - 1, 0, 100);
                break;
            case "Player 2":
                players[1] = Mathf.Clamp(players[1] - 1, 0, 100);
                break;
            case "Player 3":
                players[2] = Mathf.Clamp(players[2] - 1, 0, 100);
                break;
            case "Player 4":
                players[3] = Mathf.Clamp(players[3] - 1, 0, 100);
                break;
        }
    }

    private void CheckWinner()
    {
        bool win = false;
        bool draw = false;
        int bestPlayerId = 0;
        int bestPoint = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] >= PointsMax)
            {
                win = true;
                if (bestPoint < players[i])
                {
                    draw = false;
                    bestPlayerId = i + 1;
                    bestPoint = players[i];
                }
                else if (bestPoint == players[i])
                {
                    draw = true;
                }
            }
        }

        if (win)
        {
            if (!draw)
            {
                print("We have a winner");
                EventManager.Instance.OnPlayerWin("Player " + bestPlayerId);
                pointsScreem.SetActive(false);
            }
            else
            {
                print("We have a draw");
                EventManager.Instance.OnPlayersReady();
                pointsScreem.SetActive(false);
            }
        }
       
        else
        {
            print("Nobody win the game continues");
            EventManager.Instance.OnPlayersReady();
            pointsScreem.SetActive(false);
        }
    }

    private void UpdatePoints()
    {
        pointsScreem.SetActive(true);

        for (int i = 0; i < players.Length; i++)
        {
            playerUI[i].text = "Player " + (i + 1) + ": " + players[i].ToString();
        }

        StartCoroutine(WaitToContinue());
    }

    IEnumerator WaitToContinue()
    {
        yield return new WaitForSeconds(3f);

        CheckWinner();
    }

    private void ResetGamePoints(string player)
    {
        players = new int[4];
    }

    private void OnEnable()
    {
        EventManager.PlayerKilled += PlayerGotPoint;
        EventManager.PlayerDead += PlayerLostPoint;
        EventManager.EndMatch += UpdatePoints;
        EventManager.PlayerWin += ResetGamePoints;
    }

    private void OnDisable()
    {
        EventManager.PlayerKilled -= PlayerGotPoint;
        EventManager.PlayerDead -= PlayerLostPoint;
        EventManager.EndMatch -= UpdatePoints;
        EventManager.PlayerWin -= ResetGamePoints;
    }

}
