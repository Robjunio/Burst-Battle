using DG.Tweening;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class NetcodePrepareMap : NetworkBehaviour
{
    //[SerializeField] GameObject foam;
    [SerializeField] TMP_Text playerWinner;
    [SerializeField] TMP_Text playerWinnerShadow;

    [SerializeField] GameObject water;
    [SerializeField] GameObject map;

    [SerializeField] Vector3[] playerSpawnPosition;
    [SerializeField] Vector3[] playerSpawnRotation;

    private void StartMap()
    {
        if (!IsServer) return;

        StartMapClientRpc();

        var players = EventManager.Instance.GetNetcodePlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.SetPositionAndRotation(playerSpawnPosition[i], Quaternion.Euler(playerSpawnRotation[i]));
            players[i].ResetPlayer();
        }

    }

    [ClientRpc]
    private void StartMapClientRpc()
    {
        //foam.SetActive(false);
        //water.SetActive(true);

        water.transform.DOMoveY(-0.5f, 0.1f);
        water.tag = "Death";
        map.SetActive(true);
    }

    public void ResetWater()
    {
        if(!IsServer) return;
        water.transform.DOMoveY(-0.5f, 0.1f);
    }

    private void OnVictory(string player)
    {
        if (!IsServer) return;

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
