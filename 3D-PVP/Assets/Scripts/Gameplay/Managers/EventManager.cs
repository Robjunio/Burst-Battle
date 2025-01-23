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

    public delegate void UIEvent();
    public static event UIEvent ReachMenu;
    public static event UIEvent ReachCharacter;
    public static event UIEvent ReachGameplay;

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
        PlayersReady?.Invoke();
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

        StartMatch?.Invoke();
    }

    public List<PlayerController> GetPlayers()
    {
        return players;
    }
}
