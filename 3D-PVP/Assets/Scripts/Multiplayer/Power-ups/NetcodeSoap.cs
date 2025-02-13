using UnityEngine;
using Unity.Netcode;

public class NetcodeSoap : NetworkBehaviour, IPowerUp
{
    private GameObject SoapPrefab;
    private int durability = 1;
    private int id = 1;

    private void Awake()
    {
        SoapPrefab = Resources.Load<GameObject>("Multiplayer/Prefabs/Soap_Netcode");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir, string player)
    {
        dir = dir.normalized;

        NetworkObject powerUp = NetworkObjectPool.Singleton.GetNetworkObject(SoapPrefab, spawn.position, transform.rotation);
        powerUp.Spawn(); 

        Rigidbody body = powerUp.GetComponent<Rigidbody>();
        powerUp.name = player;

        body.AddForce(15f * dir, ForceMode.Impulse);

        durability--;
    }

    public void DestroyPowerUp()
    {
        this.enabled = false;
    }

    public void ResetDurability()
    {
        durability = 1;
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

    public int GetPowerUpID()
    {
        return id;
    }
}
