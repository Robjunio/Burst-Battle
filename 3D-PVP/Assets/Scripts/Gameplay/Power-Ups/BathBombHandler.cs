using UnityEngine;

public class BathBombHandler : MonoBehaviour, IPowerUp
{
    private GameObject BathBombPrefab;
    private int durability = 1;

    private void Awake()
    {
        BathBombPrefab = Resources.Load<GameObject>("Prefabs/DeathBomb");
    }

    public void UsePowerUp(Transform spawn, Vector2 dir)
    {
        var powerUp = Instantiate(BathBombPrefab, new Vector3(spawn.position.x + dir.x, spawn.position.y, spawn.position.z + dir.y), Quaternion.identity);

        Rigidbody body = powerUp.GetComponent<Rigidbody>();

        body.AddForce(20 * dir, ForceMode.Impulse);

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
}
