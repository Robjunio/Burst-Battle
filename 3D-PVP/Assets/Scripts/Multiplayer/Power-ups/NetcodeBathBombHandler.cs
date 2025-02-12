using UnityEngine;
using Unity.Netcode;

public class NetcodeBathBombHandler : NetworkBehaviour, IPowerUp
{
    private GameObject BathBombPrefab;
    private int durability = 1;

    private int id = 2;

    private void Awake()
    {
        BathBombPrefab = Resources.Load<GameObject>("Multiplayer/Prefabs/DeathBomb_Netcode");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir, string player)
    {
        NetworkObject powerUp = NetworkObjectPool.Singleton.GetNetworkObject(BathBombPrefab, spawn.position, transform.rotation);
        powerUp.Spawn();

        Rigidbody body = powerUp.GetComponent<Rigidbody>();
        powerUp.name = player;

        body.AddForce(15 * dir, ForceMode.Impulse);

        durability--;
    }

    public void DestroyPowerUp()
    {
        this.enabled = false;
    }

    public bool CheckDurability()
    {
        if (durability > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetDurability()
    {
        durability = 1;
    }

    public int GetPowerUpID()
    {
        return id;
    }
}
