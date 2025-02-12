using UnityEngine;
using Unity.Netcode;

public class NetcodeBubble : NetworkBehaviour
{
    private Animator _animator;
    private SphereCollider _collider;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        TryGetComponent(out _collider);
    }

    private void OnNetworkSpawn() => _collider.enabled = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PopBubbleClientRpc();
            }
        }
    }

    [ClientRpc]
    private void PopBubbleClientRpc()
    {
        _collider.enabled = false;
        _animator.SetTrigger("Pop");

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/411462__thebuilder15__bubble-pop"), Camera.main.transform.position);
    }
}
