using UnityEngine;

public class BathBombHandler : MonoBehaviour, IPowerUp
{
    private GameObject BathBombPrefab;
    private int durability = 1;

    private void Awake()
    {
        BathBombPrefab = Resources.Load<GameObject>("Prefabs/DeathBomb");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir)
    {
        var powerUp = Instantiate(BathBombPrefab, spawn.position + dir, Quaternion.identity);

        Rigidbody body = powerUp.GetComponent<Rigidbody>();

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
}
