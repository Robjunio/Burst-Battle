using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetcodePowerUp : NetworkBehaviour
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

    public void DestroyPowerUp()
    {
        NetworkObject.Despawn(true);
        NetcodePowerUpManager.PowerUpCollected?.Invoke();
    }
}

