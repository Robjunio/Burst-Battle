using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private IPowerUp powerUp;

    public IPowerUp GetPower()
    {
        return powerUp;
    }

    public void SetPower(IPowerUp power)
    {
        powerUp = power;
    }

    public void OnDisable()
    {
        PowerUpManager.PowerUpCollected?.Invoke();
    }
}
