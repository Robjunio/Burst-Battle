using DG.Tweening;
using Player;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetcodePlayerController : NetworkBehaviour
{
    [SerializeField] private Transform playerSkinParent;
    [SerializeField] private SpriteRenderer playerBase;
    private GameObject playerSkinObj;

    private Animator _animator;
    private PlayerSkin _playerSkin;

    GameObject ripple;
    GameObject deathEffect;

    NetcodePlayerAttack attackSystem;
    NetcodePlayerMovement movementSystem;

    int bubbleCount;
    public bool dead;

    private GameActions gameActions;

    private NetworkVariable<int> _powerUpCollected = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        ); 

    private void Awake()
    {
        gameActions = new GameActions();
        gameActions.Player.Enable();

        deathEffect = Resources.Load<GameObject>("Prefabs/BubbleDeathParticle"); 
        ripple = Resources.Load<GameObject>("Multiplayer/Prefabs/Netcode_RippleVFX"); 

        var list = FindObjectsOfType<NetcodePlayerController>();

        gameObject.name = "Player " + list.Length.ToString();
        playerSkinObj = Resources.Load<GameObject>("Prefabs/" + gameObject.name);

        var obj = Instantiate(playerSkinObj, playerSkinParent);

        _playerSkin = obj.GetComponent<PlayerSkin>();
        _animator = _playerSkin.GetAnimator();

        switch (list.Length)
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

        TryGetComponent(out attackSystem);
        TryGetComponent(out movementSystem);

        attackSystem.SetPlayerSkin(_playerSkin);
        attackSystem.SetInput(gameActions, movementSystem);
        movementSystem.SetAnimator(_animator);
        movementSystem.SetInput(gameActions);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (dead) return;
        if (!IsServer) return;
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
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Death"))
        {
            Die(collision.gameObject.name);
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            var powerUp = collision.gameObject.GetComponent<NetcodePowerUp>();
            if(_powerUpCollected.Value == powerUp.GetPower().GetPowerUpID())
            {
                UpdatePowerUpClientRpc();
            }
            else
            {
                _powerUpCollected.Value = powerUp.GetPower().GetPowerUpID();
            }

            powerUp.DestroyPowerUp();
        }
    }

    [ClientRpc]
    private void UpdatePowerUpClientRpc()
    {
        attackSystem.SetPowerUp(NetcodePowerUpManager.Singleton.powerUps[_powerUpCollected.Value]);
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
        dead = true;

        if (gameObject.name == player)
        {
            EventManager.Instance.OnPlayerDead(player);
            //EventManager.Instance.PlayerKillHimSelf(player);
        }
        else
        {
            EventManager.Instance.PlayerWasKilled(gameObject.name, player);
            EventManager.Instance.OnPlayerKilled(player);
        }

        if (player == "bath_water")
        {
            NetworkObject rippleEffect = NetworkObjectPool.Singleton.GetNetworkObject(ripple, new Vector3(transform.position.x, 7.1f, transform.position.z), Quaternion.Euler(90, 0, 0));
            rippleEffect.Spawn();
        }

        DieClientRpc();
    }

    [ClientRpc]
    private void DieClientRpc()
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/733264__arttim__bubble_pop"), Camera.main.transform.position);

        StartCoroutine(FreezeTime());

        Instantiate(deathEffect, transform.position, Quaternion.identity);

        attackSystem.alive = false;
        attackSystem.ResetPowerUp();

        dead = true;
        movementSystem.alive = false;

        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    private void StartPlayer()
    {
        dead = false;
        attackSystem.alive = true;
        movementSystem.Restart();

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

    private void OnDestroy() 
    {
        EventManager.StartMatch -= StartPlayer;
        gameActions.Player.Disable();
    }

    private void OnEnable()
    {
        EventManager.Instance.NetcodePlayerEnterInGame(this);
        EventManager.StartMatch += StartPlayer;
        _powerUpCollected.OnValueChanged += UpdatePowerUp;
    }

    private void UpdatePowerUp(int previousValue, int newValue)
    {
        attackSystem.SetPowerUp(NetcodePowerUpManager.Singleton.powerUps[newValue]);
    }
}
