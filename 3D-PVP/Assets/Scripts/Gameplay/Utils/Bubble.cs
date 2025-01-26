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

            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/411462__thebuilder15__bubble-pop"), Camera.main.transform.position);
        }
    }
}
