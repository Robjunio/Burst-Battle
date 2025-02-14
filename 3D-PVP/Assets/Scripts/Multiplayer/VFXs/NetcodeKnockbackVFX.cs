using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Windows;
using DG.Tweening;

public class NetcodeKnockbackVFX : NetworkBehaviour
{
    private GameObject HitEffect;
    private GameObject DeathEffectPrefab;
    [SerializeField] private NetcodePlayerController PlayerController;

    private GameObject obj;

    NetworkVariable<Vector3> moveDirection = new NetworkVariable<Vector3>(
        Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );

    private void Start()
    {
        DeathEffectPrefab = Resources.Load<GameObject>("Multiplayer/Prefabs/PlayerDeath");

        HitEffect = Resources.Load<GameObject>("Prefabs/HitVFX");

        moveDirection.OnValueChanged += ActiveKnockback;
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
     {
        if (!IsServer) return;
        if (collision.gameObject.CompareTag("Death") && collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            var dir = transform.position - collision.transform.position;
            if(dir != moveDirection.Value)
            {
                moveDirection.Value = dir;
            }
            else ActiveKnockbackClientRpc();
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (!IsServer) return;
        if (collider.gameObject.CompareTag("Death") && collider.gameObject.GetComponent<Rigidbody>() != null)
        {
            var dir = transform.position - collider.transform.position;
            if (dir != moveDirection.Value)
            {
                moveDirection.Value = dir;
            }
            else ActiveKnockbackClientRpc();
        }
    }

    [ClientRpc]
    private void ActiveKnockbackClientRpc()
    {
        var hit = Instantiate(HitEffect, transform.position, Quaternion.identity);
        obj = Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);

        var player = Instantiate(transform.GetChild(1).gameObject, obj.transform);
        player.SetActive(true);

        var rot = Quaternion.LookRotation(moveDirection.Value * -1, Vector3.up);

        player.transform.rotation = rot;

        player.transform.DOShakePosition(0.2f, 0.7f, 60);

        StartCoroutine(AddForce());
    }

    private void ActiveKnockback(Vector3 oldValue, Vector3 newValue)
    {
        var hit = Instantiate(HitEffect, transform.position, Quaternion.identity);
        obj = Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);

        var player = Instantiate(transform.GetChild(1).gameObject, obj.transform);
        player.SetActive(true);

        var rot = Quaternion.LookRotation(newValue * -1, Vector3.up);

        player.transform.rotation = rot;

        player.transform.DOShakePosition(0.2f, 0.7f, 60);

        StartCoroutine(AddForce());
    }

    IEnumerator AddForce()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        var rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(moveDirection.Value.normalized * 1500f);
        rb.AddTorque(moveDirection.Value);
        gameObject.layer = LayerMask.NameToLayer("DeadLayer");
    }
    private void OnDestroy()
    {
        moveDirection.OnValueChanged -= ActiveKnockback;
    }
}
