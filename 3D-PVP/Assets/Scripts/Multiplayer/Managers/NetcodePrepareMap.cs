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
    [SerializeField] Transform mapTransform;
    NetworkObject map;

    [SerializeField] Vector3[] playerSpawnPosition;
    [SerializeField] Vector3[] playerSpawnRotation;

    private void StartMap()
    {
        if (!IsServer) return;

        StartMapClientRpc();

        if (map == null)
        {
            GameObject mapPrefab = Resources.Load<GameObject>("Multiplayer/Prefabs/Foam");

            map = NetworkObjectPool.Singleton.GetNetworkObject(mapPrefab, mapTransform.position, mapTransform.rotation);
            map.transform.localScale = mapTransform.localScale * 10;
            map.Spawn();
        }
        

        var players = EventManager.Instance.GetNetcodePlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.SetPositionAndRotation(playerSpawnPosition[i], Quaternion.Euler(playerSpawnRotation[i]));
        }

        ResetPlayersClientRpc();
    }
    [ClientRpc]
    private void ResetPlayersClientRpc()
    {
        var players = EventManager.Instance.GetNetcodePlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].ResetPlayer();
        }
    }

    [ClientRpc]
    private void StartMapClientRpc()
    {
        //foam.SetActive(false);
        //water.SetActive(true);

        water.transform.DOMoveY(-0.4f, 0.1f);
        water.tag = "Death";
    }

    public void ResetWater()
    {
        water.transform.DOMoveY(-0.4f, 0.1f);
    }

    private void OnVictory(string player)
    {
        if (!IsServer) return;

        map.Despawn();
        map = null;

        OnVictoryClientRpc();
    }

    [ClientRpc]
    private void OnVictoryClientRpc()
    {
        water.tag = "Player";
        water.transform.DOMoveY(-5f, 1f).OnComplete(() =>
        {
            EventManager.Instance.OnReachVictory();
        });

        playerWinner.text = "Winner\n" + "Player " + NetcodePointsManager.Singleton.GetWinner();
        playerWinnerShadow.text = "Winner\n" + "Player " + NetcodePointsManager.Singleton.GetWinner();
    }

    private void OnEnable()
    {
        EventManager.PlayersReady += StartMap;
        EventManager.PlayerWin += OnVictory;
        EventManager.ReachMenu += ResetWater;
    }

    private void OnDisable()
    {
        EventManager.PlayersReady -= StartMap;
        EventManager.PlayerWin -= OnVictory;
        EventManager.ReachMenu -= ResetWater;

    }
}
