using UnityEngine;

public class BubbleGun : MonoBehaviour, IPowerUp
{
    private GameObject BubblePrefab;
    private int durability = 12;

    private void Awake()
    {
        BubblePrefab = Resources.Load<GameObject>("Prefabs/Bubble");
    }

    public void UsePowerUp(Transform spawn, Vector3 dir)
    {
        dir = BubbleAimDiff(dir);
        var powerUp = Instantiate(BubblePrefab, spawn.position + dir, Quaternion.identity);

        Rigidbody body = powerUp.GetComponent<Rigidbody>();

        body.AddForce(2.5f * dir, ForceMode.Impulse);

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

    private Vector3 BubbleAimDiff(Vector3 aim)
    {
        float x = Random.Range(-0.3f, 0.3f);

        float z = Random.Range(-0.3f, 0.3f);

        var dir = new Vector3(aim.x + x, 0f, aim.z + z);

        return dir.normalized;
    }
}
