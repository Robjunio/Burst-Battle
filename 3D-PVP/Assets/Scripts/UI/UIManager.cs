using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Character;
    [SerializeField] GameObject VictoryScreen;

    private void StartMenu()
    {
        Menu.SetActive(true);
    }

    private void StartCharacter()
    {
        Character.SetActive(true);
    }

    private void StartVictoryScreen() 
    { 
        VictoryScreen.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
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
