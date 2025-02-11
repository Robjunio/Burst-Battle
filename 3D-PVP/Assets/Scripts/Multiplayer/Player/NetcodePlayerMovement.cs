using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class NetcodePlayerMovement : NetworkBehaviour
    {
        public bool alive = false;

        private Rigidbody _rb;
        private Animator _animator;

        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _dashForce = 16;
        private float _turnSpeed = 720;
        private Vector3 _input;

        private bool _canDash = true;
        private float _timmer;
        private float _dashCooldown = 0.5f;

        private Vector3 currentScale = Vector3.one;

        private NetworkVariable<Vector2> _currentInput = new NetworkVariable<Vector2>(
            Vector2.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
            );

        private GameActions gameActions;

        private void Awake()
        {
            TryGetComponent(out _rb);
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void SetInput(GameActions gameActions)
        {
            this.gameActions = gameActions;
            gameActions.Player.Dash.performed += OnDash;
        }

        public Vector3 GetInput()
        {
            return _input;
        }

        public void GetMovement()
        {
            if (!alive) return;

            if (IsOwner)
            {
                Vector2 dir = gameActions.Player.Move.ReadValue<Vector2>();

                dir = dir.normalized;

                if (dir != _currentInput.Value)
                {
                    _currentInput.Value = dir;
                }
            }

            _input = new Vector3(_currentInput.Value.x, 0, _currentInput.Value.y);

            _animator.SetFloat("Magnitude", _input.magnitude);
        }

        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (!alive || !IsOwner) return;
            if (!_canDash)
            {
                var diff = Time.time - _timmer;
                if (diff > _dashCooldown)
                {
                    _canDash = true;
                    OnDash(ctx);
                }
                return;
            }

            _animator.SetTrigger("Dash");

            currentScale = transform.localScale;

            _rb.velocity = Vector3.zero;
            if (_input != Vector3.zero)
            {
                var rot = Quaternion.LookRotation(_input, Vector3.up);

                transform.rotation = rot;
            }

            _rb.AddForce(transform.forward * _dashForce  * transform.localScale.x, ForceMode.Impulse);

            _timmer = Time.time;
            _canDash = false;
            _rb.mass = 1 * transform.localScale.x;
        }

        private void Update()
        {
            Look();
            GetMovement();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Look()
        {
            if (_input == Vector3.zero) return;

            var rot = Quaternion.LookRotation(_input, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }

        private void Move()
        {
            var lerpPos = transform.position + _input * _movementSpeed * Time.deltaTime;
            _rb.MovePosition(lerpPos);

            _rb.angularVelocity = Vector3.zero;
        }

        public void Reset()
        {
            _input = Vector3.zero;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            if (!IsServer) return;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }

        public void Restart()
        {
            alive = true;

            if (!IsServer) return;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private void OnDisable()
        {
            gameActions.Player.Dash.performed -= OnDash;
        }
    }
}
