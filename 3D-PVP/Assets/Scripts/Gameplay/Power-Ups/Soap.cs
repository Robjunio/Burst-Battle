using UnityEngine;

public class Soap : MonoBehaviour, IPowerUp
{
    private GameObject SoapPrefab;
    private int durability = 1;

    private void Awake()
    {
        SoapPrefab = Resources.Load<GameObject>("Prefabs/Soap");
    }

    public void UsePowerUp(Transform spawn, Vector2 dir)
    {
        var powerUp = Instantiate(SoapPrefab, new Vector3(spawn.position.x + dir.x, spawn.position.y, spawn.position.z + dir.y), Quaternion.Euler(90, 0, 0));

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
