using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Character;
    [SerializeField] GameObject VictoryScreen;
    [SerializeField] AudioSource AudioSource;

    private void Awake()
    {
        Singleton = this;
    }

    private void StartMenu()
    {
        Menu.SetActive(true);
    }

    private void StartCharacter()
    {
        Character.SetActive(true);
    }

    public void DisableStartCharacter()
    {
        Character.SetActive(false);
    }

    private void StartVictoryScreen() 
    { 
        VictoryScreen.SetActive(true);
    }
    
    public void DisableVictoryScreen()
    {
        VictoryScreen.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnSelect()
    {
        AudioSource.Play();
    }

    private void OnEnable()
    {
        EventManager.ReachCharacter += StartCharacter;
        EventManager.ReachMenu += StartMenu;
        EventManager.ReachVictory += StartVictoryScreen;
    }

    private void OnDisable()
    {
        EventManager.ReachMenu -= StartMenu;
        EventManager.ReachCharacter -= StartCharacter;
        EventManager.ReachVictory -= StartVictoryScreen;
    }
}
