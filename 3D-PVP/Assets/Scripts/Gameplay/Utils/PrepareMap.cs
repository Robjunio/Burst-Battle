using DG.Tweening;
using TMPro;
using UnityEngine;

public class PrepareMap : MonoBehaviour
{
    //[SerializeField] GameObject foam;
    [SerializeField] TMP_Text playerWinner;
    [SerializeField] TMP_Text playerWinnerShadow;

    [SerializeField] GameObject water;
    [SerializeField] GameObject map;

    [SerializeField] Vector3[] playerSpawnPosition;

    private void StartMap() 
    {
        //foam.SetActive(false);
        //water.SetActive(true);
        water.tag = "Death";
        map.SetActive(true);

        var players = EventManager.Instance.GetPlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = playerSpawnPosition[i];
            players[i].transform.rotation = Quaternion.identity;
            var rb = players[i].gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            players[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnVictory(string player)
    {
        map.SetActive(false);
        water.tag = "Player";
        water.transform.DOMoveY(-5f, 1f).OnComplete(() =>
        {
            EventManager.Instance.OnReachVictory();
        });

        playerWinner.text = "Winner\n" + player;
        playerWinnerShadow.text = "Winner\n" + player;
    }

    private void OnEnable()
    {
        EventManager.PlayersReady += StartMap;
        EventManager.PlayerWin += OnVictory;
    }

    private void OnDisable()
    {
        EventManager.PlayersReady -= StartMap;
        EventManager.PlayerWin -= OnVictory;
    }
}
