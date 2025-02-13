using UnityEngine;
using Unity.Netcode;
public class NetcodeBubbleGun : NetworkBehaviour, IPowerUp
{
    private GameObject BubblePrefab;
    private int durability = 12;

    private int id = 0;

    private void Awake()
    {
        BubblePrefab = Resources.Load<GameObject>("Multiplayer/Prefabs/Bubble_Netcode");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir, string player)
    {
        dir = BubbleAimDiff(dir);

        PlayAudioClientRpc();
        NetworkObject powerUp = NetworkObjectPool.Singleton.GetNetworkObject(BubblePrefab, spawn.position, transform.rotation);
        powerUp.Spawn();

        powerUp.name = player;
        Rigidbody body = powerUp.GetComponent<Rigidbody>();

        body.AddForce(2.5f * dir, ForceMode.Impulse);

        durability--;
    }

    [ClientRpc]
    private void PlayAudioClientRpc()
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/376968__elmasmalo1__bubble-pop"), Camera.main.transform.position);
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
        durability = 12;
    }

    private Vector3 BubbleAimDiff(Vector3 aim)
    {
        float x = Random.Range(-0.3f, 0.3f);

        float z = Random.Range(-0.3f, 0.3f);

        var dir = new Vector3(aim.x + x, 0f, aim.z + z);

        return dir.normalized;
    }

    public int GetPowerUpID()
    {
        return id;
    }
}
