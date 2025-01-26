using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    [SerializeField] AudioClip[] musicClips;
    public static EventManager Instance; 

    List<PlayerController> players = new List<PlayerController>();

    List<int> player1KillsIds = new List<int>();
    List<int> player2KillsIds = new List<int>();
    List<int> player3KillsIds = new List<int>();
    List<int> player4KillsIds = new List<int>();

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
            /*foreach (var cont in players)
            {
                if (!cont.dead)
                {
                    OnPlayerSurvived(cont.gameObject.name);
                }
            }*/
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

        player1KillsIds.Clear();
        player2KillsIds.Clear();
        player3KillsIds.Clear();
        player4KillsIds.Clear();
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

    public void PlayerWasKilled(string playerKilled, string playerKiller)
    {
        int playerId = 0;
        switch (playerKilled)
        {
            case "Player 1":
                playerId = 0;
                break;
            case "Player 2":
                playerId = 1;
                break;
            case "Player 3":
                playerId = 2;
                break;
            case "Player 4":
                playerId = 3;
                break;
        }

        switch (playerKiller)
        {
            case "Player 1":
                player1KillsIds.Add(playerId);
                break;
            case "Player 2":
                player2KillsIds.Add(playerId);
                break;
            case "Player 3":
                player3KillsIds.Add(playerId);
                break;
            case "Player 4":
                player4KillsIds.Add(playerId);
                break;
        }
    }

    public void PlayerKillHimSelf(string player)
    {
        switch (player)
        {
            case "Player 1":
                if(player1KillsIds.Count > 0) { 
                    player1KillsIds.RemoveAt(player1KillsIds.Count - 1);
                }
                break;
            case "Player 2":
                if (player2KillsIds.Count > 0)
                {
                    player2KillsIds.RemoveAt(player2KillsIds.Count - 1);
                }
                break;
            case "Player 3":
                if (player3KillsIds.Count > 0)
                {
                    player3KillsIds.RemoveAt(player3KillsIds.Count - 1);
                }
                break;
            case "Player 4":
                if (player4KillsIds.Count > 0)
                {
                    player4KillsIds.RemoveAt(player4KillsIds.Count - 1);
                }
                break;
        }
    }

    public List<int> GetPlayer1Kills()
    {
        return player1KillsIds;
    }
    public List<int> GetPlayer2Kills()
    {
        return player2KillsIds;
    }
    public List<int> GetPlayer3Kills()
    {
        return player3KillsIds;
    }
    public List<int> GetPlayer4Kills()
    {
        return player4KillsIds;
    }
}
