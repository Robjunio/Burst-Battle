using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance; 

    List<PlayerController> players = new List<PlayerController>();

    public delegate void GameEvent();
    public static event GameEvent PlayerEnter;
    public static event GameEvent PlayersReady;
    public static event GameEvent StartMatch;
    public static event GameEvent EndMatch;

    public delegate void Battle(string player);
    public static event Battle PlayerKilled;
    public static event Battle PlayerDead;
    public static event Battle PlayerSurvived;
    public static event Battle PlayerWin;


    public delegate void UIEvent();
    public static event UIEvent ReachMenu;
    public static event UIEvent ReachCharacter;
    public static event UIEvent ReachGameplay;
    public static event UIEvent ReachVictory;

    private int playersCount;
    private bool matchEnded;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerEnterInGame(PlayerController controller)
    {
        players.Add(controller);

        PlayerEnter?.Invoke();
    }

    public void OnPlayersReady()
    {
        playersCount = players.Count;
        PlayersReady?.Invoke();
    }

    public void OnMatchEnded()
    {
        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(1f);

        EndMatch?.Invoke();
    }

    // Player kills somebody 
    public void OnPlayerKilled(string player)
    {
        if(matchEnded) return;
        PlayerKilled?.Invoke(player);
        playersCount--;
        if(playersCount <= 1)
        {
            matchEnded = true;
            foreach (var cont in players)
            {
                if (!cont.dead)
                {
                    OnPlayerSurvived(cont.gameObject.name);
                }
            }
            OnMatchEnded();
        }
    }

    // Player kills himself
    public void OnPlayerDead(string player) 
    {
        if (matchEnded) return;
        PlayerDead?.Invoke(player);
        playersCount--;
        if (playersCount <= 1)
        {
            matchEnded = true;
            foreach (var cont in players)
            {
                if (!cont.dead)
                {
                    OnPlayerSurvived(cont.gameObject.name);
                }
            }
            OnMatchEnded();
        }
    }

    public void OnPlayerSurvived(string player)
    {
        PlayerSurvived?.Invoke(player);
    }

    public void OnReachMenu()
    {
        ReachMenu?.Invoke();
    }

    public void OnReachCharacter()
    {
        ReachCharacter?.Invoke();
    }

    public void OnReachGameplay()
    {
        ReachGameplay?.Invoke();
    }

    public void OnMatchStarted()
    {
        StartMatch?.Invoke();
        matchEnded = false;
    }

    public void OnReachVictory()
    {
        ReachVictory?.Invoke();
    }

    public void OnPlayerWin(string player)
    {
        PlayerWin?.Invoke(player);
    }

    public List<PlayerController> GetPlayers()
    {
        return players;
    }
}
