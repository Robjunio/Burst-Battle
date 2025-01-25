using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterSelectionHandler : MonoBehaviour
{
    [SerializeField] private Image[] playerImages;
    [SerializeField] private GameObject[] characterPressToStart;
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private Sprite baseSprite;

    private void HandlePlayerInGame()
    {
        var players = EventManager.Instance.GetPlayers();
        for (int i = 0; i < players.Count; i++)
        {
            playerImages[i].sprite = characterSprites[i];
            characterPressToStart[i].SetActive(false);
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
        for (int i = 0; i < playerImages.Length; i++)
        {
            playerImages[i].sprite = baseSprite;
            characterPressToStart[i].SetActive(true);
        }
    }
}
