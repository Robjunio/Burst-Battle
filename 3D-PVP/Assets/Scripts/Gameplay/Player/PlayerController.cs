using DG.Tweening;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerSkinParent;
    [SerializeField] private SpriteRenderer playerBase;
    private GameObject playerSkinObj;

    private Animator _animator;
    private PlayerSkin _playerSkin;

    GameObject ripple;
    GameObject deathEffect;

    PlayerAttack attackSystem;
    PlayerMovement movementSystem;

    int bubbleCount;
    public bool dead;
    
    private void Awake()
    {
        deathEffect = Resources.Load<GameObject>("Prefabs/BubbleDeathParticle");
        ripple = Resources.Load<GameObject>("Prefabs/RippleVFX");

        var list = FindObjectsOfType<PlayerController>();

        TryGetComponent(out attackSystem);
        TryGetComponent(out movementSystem);

        gameObject.name = "Player " + list.Length.ToString();
        playerSkinObj = Resources.Load<GameObject>("Prefabs/" + gameObject.name);

        var obj = Instantiate(playerSkinObj, playerSkinParent);

        _playerSkin = obj.GetComponent<PlayerSkin>();
        _animator = _playerSkin.GetAnimator();

        attackSystem.SetPlayerSkin(_playerSkin);
        movementSystem.SetAnimator(_animator);

        switch(list.Length)
        {
            case 1:
                playerBase.color = Color.blue;
                break;
            case 2:
                playerBase.color = Color.magenta;
                break;
            case 3:
                playerBase.color = Color.green;
                break;
            case 4:
                playerBase.color = Color.yellow;
                break;
        }
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
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (dead) return;
        if (collision.gameObject.CompareTag("Death"))
        {
            Die(collision.gameObject.name);
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
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/maximize_006"), Camera.main.transform.position);
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

    IEnumerator FreezeTime()
    {
        yield return null;
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;
    }

    private void Die(string player)
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/733264__arttim__bubble_pop"), Camera.main.transform.position);
        StartCoroutine(FreezeTime());
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        dead = true;
        attackSystem.alive = false;
        attackSystem.ResetPowerUp();

        movementSystem.alive = false;

        if(gameObject.name == player) 
        {
            EventManager.Instance.OnPlayerDead(player);
            //EventManager.Instance.PlayerKillHimSelf(player);
        }
        else
        {
            if(player == "bath_water") 
            { 
                Instantiate(ripple, new Vector3(transform.position.x, 7f, transform.position.z), Quaternion.Euler(90,0,0));
            }

            EventManager.Instance.PlayerWasKilled(gameObject.name, player);
            EventManager.Instance.OnPlayerKilled(player);
        }
        
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    private void StartPlayer()
    {
        dead = false;
        attackSystem.alive = true;
        movementSystem.alive = true;

        bubbleCount = 0;

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void ResetPlayer()
    {
        attackSystem.alive = false;
        movementSystem.alive = false;

        movementSystem.Reset();
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.localScale = Vector3.one;
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
