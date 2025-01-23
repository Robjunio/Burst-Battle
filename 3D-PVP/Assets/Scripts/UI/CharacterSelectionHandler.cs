using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionHandler : MonoBehaviour
{
    [SerializeField] private Image[] playerImages;

    private void HandlePlayerInGame()
    {
        var players = EventManager.Instance.GetPlayers();
        for (int i = 0; i < players.Count; i++)
        {
            playerImages[i].color = Color.green;
        }
    }

    public void StartBattle()
    {
        EventManager.Instance.OnPlayersReady();
    }

    private void OnEnable()
    {
        HandlePlayerInGame();
        EventManager.PlayerEnter += HandlePlayerInGame;
    }

    private void OnDisable()
    {
        EventManager.PlayerEnter -= HandlePlayerInGame;
        foreach (var player in playerImages)
        {
            player.color = Color.white;
        }
    }
}
