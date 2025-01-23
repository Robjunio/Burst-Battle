using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerAttack attackSystem;
    PlayerMovement movementSystem;
    int bubbleCount;
    bool dead;
    
    private void Awake()
    {
        var list = FindObjectsOfType<PlayerController>();

        TryGetComponent(out attackSystem);
        TryGetComponent(out movementSystem);

        gameObject.name = "Player " + list.Length.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(dead) return;
        if (collision.gameObject.CompareTag("Death"))
        {
            Die(collision.gameObject.name);
        }

        if (collision.gameObject.CompareTag("Bubble"))
        {
            HandlerBubbleAttack(collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (dead) return;
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

    private void HandlerBubbleAttack(string player)
    {
        bubbleCount++;
        if(bubbleCount >= 3)
        {
            Die(player);
        }
        else
        {
            transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void Die(string player)
    {
        dead = true;
        attackSystem.alive = false;
        attackSystem.ResetPowerUp();

        movementSystem.alive = false;

        if(gameObject.name == player) 
        {
            EventManager.Instance.OnPlayerDead(player);
        }
        else
        {
            EventManager.Instance.OnPlayerKilled(player);
        }
        
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void StartPlayer()
    {
        dead = false;
        attackSystem.alive = true;
        movementSystem.alive = true;

        transform.localScale = Vector3.one;
        bubbleCount = 0;

        transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        EventManager.StartMatch -= StartPlayer;
    }

    private void OnEnable()
    {
        EventManager.Instance.PlayerEnterInGame(this);
        EventManager.StartMatch += StartPlayer;
    }
}
