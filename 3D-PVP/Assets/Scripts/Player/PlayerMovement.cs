using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody _rb;
        private PlayerInput _playerInput;

        private float _movementSpeed;
        private float _dashForce = 500;
        private float _turnSpeed = 720;
        private Vector3 _input;
        private float _currentAngle;

        private bool _canDash = true;
        private float _timmer;
        private float _dashCooldown = 0.5f;

        public void SetInfo(PlayerInput input, Rigidbody rb, float movementSpeed)
        {
            _playerInput = input;
            _movementSpeed = movementSpeed;
            _rb = rb;

            _playerInput.gameActions.Player.Dash.performed += OnDash;
        }

        private void Update()
        {
            _input = _playerInput.GetMovement();

            Look();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Look()
        {
            if (_input == Vector3.zero) return;

            var rot = Quaternion.LookRotation(transform.position, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }

        private void Move()
        {
            var lerpPos = Vector3.Lerp(transform.position, transform.position * _movementSpeed * Time.deltaTime, 0.8f);
            _rb.velocity = Vector3.Lerp(_rb.velocity, new Vector3(0, _rb.velocity.y, 0), 0.1f);
            _rb.MovePosition(lerpPos);
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (!_canDash)
            {
                var diff = Time.time - _timmer;
                if (diff > _dashCooldown) { 
                    _canDash = true;
                    OnDash(context);
                }
                return;
            }

            _rb.velocity = Vector3.zero;
            if (_input != Vector3.zero)
            {
                var rot = Quaternion.LookRotation(transform.position, Vector3.up);

                transform.rotation = rot;
            }
            _rb.AddForce(transform.forward * _dashForce, ForceMode.Impulse);
            _timmer = Time.time;
            _canDash = false;
        }

        private void OnEnable()
        {
            if (_playerInput == null) return;
            _playerInput.gameActions.Player.Dash.performed += OnDash;
        }

        private void OnDisable()
        {
            _playerInput.gameActions.Player.Dash.performed -= OnDash;
        }
    }
}
