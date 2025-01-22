using UnityEngine;

public class Soap : MonoBehaviour, IPowerUp
{
    private GameObject SoapPrefab;
    private int durability = 1;

    private void Awake()
    {
        SoapPrefab = Resources.Load<GameObject>("Prefabs/Soap");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir)
    {
        dir = dir.normalized;
        var powerUp = Instantiate(SoapPrefab, spawn.position + dir, Quaternion.Euler(90, 0, 0));

        Rigidbody body = powerUp.GetComponent<Rigidbody>();

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
}
