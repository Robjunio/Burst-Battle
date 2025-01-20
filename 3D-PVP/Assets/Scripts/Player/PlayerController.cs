using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;

        private PlayerInput _input;
        private PlayerMovement _movement;
        private PlayerAttack _attack;

        private Rigidbody _rb;

        private void Awake()
        {
            TryGetComponent(out _rb);

            _input = gameObject.AddComponent<PlayerInput>();

            _attack = gameObject.AddComponent<PlayerAttack>();
            _attack.StartPlayerAttack(_input, _rb);

            _movement = gameObject.AddComponent<PlayerMovement>();
            _movement.SetInfo(_input, _rb, speed);
        }
    }
}

