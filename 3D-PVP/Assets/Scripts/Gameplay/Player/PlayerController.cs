using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerAttack attackSystem;
    int bubbleCount;
    
    private void Awake()
    {
        var list = FindObjectsOfType<PlayerController>();

        TryGetComponent(out attackSystem);

        gameObject.name = "Player " + list.Length.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            print("Died");
            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Bubble"))
        {
            HandlerBubbleAttack();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            var powerUp = collision.gameObject.GetComponent<PowerUp>();
            attackSystem.SetPowerUp(powerUp.GetPower());
            Destroy(collision.gameObject);
        }
    }

    private void HandlerBubbleAttack()
    {
        bubbleCount++;
        if(bubbleCount >= 3)
        {
            print("Died");
            this.gameObject.SetActive(false);
        }
        else
        {
            transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
        bubbleCount = 0;
    }
}
