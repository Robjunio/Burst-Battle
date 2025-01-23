using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareMap : MonoBehaviour
{
    [SerializeField] GameObject foam;
    [SerializeField] GameObject water;
    [SerializeField] GameObject map;

    [SerializeField] Vector3[] playerSpawnPosition;

    private void StartMap() 
    {
        foam.SetActive(false);
        //water.SetActive(true);
        map.SetActive(true);

        var players = EventManager.Instance.GetPlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = playerSpawnPosition[i];
            players[i].gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;


            players[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        EventManager.PlayersReady += StartMap;
    }

    private void OnDisable()
    {
        EventManager.PlayersReady -= StartMap;
    }
}
