using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
public class NetcodeCharacterSelectionHandler : NetworkBehaviour
{
    [SerializeField] private Image[] playerImages;
    [SerializeField] private GameObject[] characterPressToStart;
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private Sprite baseSprite;

    [SerializeField] private GameObject startGameButton;
    [SerializeField] private TMP_Text codeText;

    [SerializeField] private TMP_InputField codeInput;


    private void Start()
    {
        var players = EventManager.Instance.GetNetcodePlayers().Count; 
        
        if (players == 1)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }
    private void HandlePlayerInGame()
    {
        if (codeInput.text != string.Empty)
            {
                codeText.text = codeInput.text;
            }

        var players = EventManager.Instance.GetNetcodePlayers().Count;

        
        for (int i = 0; i < players; i++)
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
