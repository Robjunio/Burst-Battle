using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public delegate void PowerUpEvent();
    public static PowerUpEvent PowerUpCollected;

    private GameObject PowerUpPrefab;
    private List<IPowerUp> powerUps = new List<IPowerUp>();
    [SerializeField] private Vector3[] startingPoints;

    private int currentPowerUpsOnMap;

    private Coroutine powerUpCoroutine;

    private Vector3 normalPowerUpPosition = new Vector3(12, 8.5f, 10);
    void Awake()
    {
        PowerUpPrefab = Resources.Load<GameObject>("Prefabs/PowerUp");

        var powerUp1 = GetComponent<Soap>();
        var powerUp2 = GetComponent<BathBombHandler>();
        var powerUp3 = GetComponent<BubbleGun>();

        powerUps.Add(powerUp1);
        powerUps.Add(powerUp2);
        powerUps.Add(powerUp3);
    }

    private void StartPowerUps()
    {
        foreach(Vector3 pos in startingPoints)
        {
            InstancePowerUp(pos);
        }
    }

    private void RemovePowerUp()
    {
        currentPowerUpsOnMap--;
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

        var obj = Instantiate(PowerUpPrefab, position, Quaternion.identity);

        var powerUp = obj.GetComponent<PowerUp>();

        powerUps[randomPowerUpID].ResetDurability();
        powerUp.SetPower(powerUps[randomPowerUpID]);

        currentPowerUpsOnMap++;
    }

    // Update is called once per frame
    private void OnEnable()
    {
        PowerUpCollected += RemovePowerUp;
        EventManager.PlayersReady += StartPowerUps;
    }

    private void OnDisable()
    {
        PowerUpCollected -= RemovePowerUp;
        EventManager.PlayersReady -= StartPowerUps;
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
    }
}
