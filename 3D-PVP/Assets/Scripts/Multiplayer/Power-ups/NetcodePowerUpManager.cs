using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class NetcodePowerUpManager : NetworkBehaviour
{
    public static NetcodePowerUpManager Singleton;

    public delegate void PowerUpEvent();
    public static PowerUpEvent PowerUpCollected;

    private GameObject PowerUpPrefab;
    public List<IPowerUp> powerUps = new List<IPowerUp>();
    [SerializeField] private Vector3[] startingPoints;

    private List<NetworkObject> powerUpsObj = new List<NetworkObject>();

    private int currentPowerUpsOnMap;

    private Coroutine powerUpCoroutine;

    private Vector3 normalPowerUpPosition = new Vector3(12, 8.5f, 10);

    bool canCreate;
    void Awake()
    {
        Singleton = this;

        PowerUpPrefab = Resources.Load<GameObject>("Multiplayer/Prefabs/Netcode_PowerUp");

        var powerUp1 = GetComponent<NetcodeSoap>();
        var powerUp2 = GetComponent<NetcodeBathBombHandler>();
        var powerUp3 = GetComponent<NetcodeBubbleGun>();

        powerUps.Add(powerUp3);
        powerUps.Add(powerUp1);
        powerUps.Add(powerUp2);
    }

    private void StartPowerUps()
    {
        if (IsServer)
        {
            canCreate = true;
            foreach (Vector3 pos in startingPoints)
            {
                InstancePowerUp(pos);
            }
        }
    }

    private void RemovePowerUp()
    {
        if (!IsServer) return;

        currentPowerUpsOnMap--;

        if (!canCreate) return;
        
        if(currentPowerUpsOnMap <= 0)
        {
            powerUpCoroutine = StartCoroutine(RestartPowerUp());
        }
    }

    IEnumerator RestartPowerUp()
    {
        yield return new WaitForSeconds(2f);
        InstancePowerUp(normalPowerUpPosition);
    }

    private void InstancePowerUp(Vector3 position)
    {
        int randomPowerUpID = Random.Range(0, powerUps.Count);

        //Acessar a Pool e recolher o objeto
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(PowerUpPrefab, position, transform.rotation);
        obj.Spawn(); //Criar o objeto na rede - Server
        
        powerUpsObj.Add(obj);

        var powerUp = obj.GetComponent<NetcodePowerUp>();

        powerUps[randomPowerUpID].ResetDurability();
        powerUp.SetPower(powerUps[randomPowerUpID]);

        currentPowerUpsOnMap++;
    }

    private void Reset()
    {
        if(!IsServer) return;
        canCreate = false;

        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        for (int i = 0; i < powerUpsObj.Count; i++)
        {
            if (powerUpsObj[i].IsSpawned)
            {
                powerUpsObj[i].Despawn(true);
            }
        }

        currentPowerUpsOnMap = 0;
        powerUpsObj.Clear();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        PowerUpCollected += RemovePowerUp;
        EventManager.PlayersReady += StartPowerUps;
        EventManager.EndMatch += Reset;
    }

    private void OnDisable()
    {
        PowerUpCollected -= RemovePowerUp;
        EventManager.PlayersReady -= StartPowerUps;
        EventManager.EndMatch -= Reset;

        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
    }
}
