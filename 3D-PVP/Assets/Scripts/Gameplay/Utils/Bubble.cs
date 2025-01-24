using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Animator _animator;
    private SphereCollider _collider;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        TryGetComponent(out _collider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collider.enabled = false;
            _animator.SetTrigger("Pop");
        }
    }
}
