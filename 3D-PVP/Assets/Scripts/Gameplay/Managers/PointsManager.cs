using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private GameObject pointsScreem;
    [SerializeField] private GameObject transition;

    [SerializeField] private Image[] player1Points;
    [SerializeField] private Image[] player2Points;
    [SerializeField] private Image[] player3Points;
    [SerializeField] private Image[] player4Points;

    [SerializeField] private Sprite[] killSprite;

    [SerializeField] private Sprite baseSprite;

    private int PointsMax = 10;
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

    private void PlayerSurvived(string player)
    {
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
                StartCoroutine(SetWinner("Player " + bestPlayerId));
            }
            else
            {
                print("We have a draw");
                StartCoroutine(ContinueMatch());
            }
        }
       
        else
        {
            print("Nobody win the game continues");
            StartCoroutine(ContinueMatch());
        }
    }

    IEnumerator ContinueMatch()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(0.67f);
        pointsScreem.SetActive(false);
        EventManager.Instance.OnPlayersReady();

        yield return new WaitForSeconds(1f);

        transition.SetActive(false);
    }

    IEnumerator SetWinner(string winner)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(0.67f);
        pointsScreem.SetActive(false);
        EventManager.Instance.OnPlayerWin(winner);

        yield return new WaitForSeconds(1f);

        transition.SetActive(false);
    }

    private void UpdatePoints()
    {
        var p1 = EventManager.Instance.GetPlayer1Kills();
        var p2 = EventManager.Instance.GetPlayer2Kills();
        var p3 = EventManager.Instance.GetPlayer3Kills();
        var p4 = EventManager.Instance.GetPlayer4Kills();

        pointsScreem.SetActive(true);

        for(int i = 0;i < PointsMax;i++)
        {
            if (i <= p1.Count - 1) {
                player1Points[i].sprite = killSprite[p1[i]];
            }
            else
            {
                player1Points[i].sprite = baseSprite;
            }

            if (i <= p2.Count - 1)
            {
                player2Points[i].sprite = killSprite[p2[i]];
            }
            else
            {
                player2Points[i].sprite = baseSprite;
            }

            if (i <= p3.Count - 1)
            {
                player3Points[i].sprite = killSprite[p3[i]];
            }
            else
            {
                player3Points[i].sprite = baseSprite;
            }

            if (i <= p4.Count - 1)
            {
                player4Points[i].sprite = killSprite[p4[i]];
            }
            else
            {
                player4Points[i].sprite = baseSprite;
            }
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
        EventManager.PlayerSurvived += PlayerSurvived;
    }

    private void OnDisable()
    {
        EventManager.PlayerKilled -= PlayerGotPoint;
        EventManager.PlayerDead -= PlayerLostPoint;
        EventManager.EndMatch -= UpdatePoints;
        EventManager.PlayerWin -= ResetGamePoints;
        EventManager.PlayerSurvived -= PlayerSurvived;
    }

}
