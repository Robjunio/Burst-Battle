using UnityEngine;

public class Soap : MonoBehaviour, IPowerUp
{
    private GameObject SoapPrefab;
    private int durability = 1;
    private int id = 1;

    private void Awake()
    {
        SoapPrefab = Resources.Load<GameObject>("Prefabs/Soap");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir, string player)
    {
        dir = dir.normalized;
        var powerUp = Instantiate(SoapPrefab, spawn.position + dir, Quaternion.identity);

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
