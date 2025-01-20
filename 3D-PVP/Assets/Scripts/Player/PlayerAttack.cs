using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private Rigidbody _rb;

        private float _meleeComboCooldown = 0.3f;

        private int _meleeCombo = 0;
        private float _meleeDamage;
        private float _lastAttackTime;

        private int _arrowsPerShoot = 1;
        private int _enemiesToBePierced = 1;
        private float _ramgedDamage;
        private float _rangedMaxCooldown;

        private bool _isAttacking = false;

        public void StartPlayerAttack(PlayerInput playerInput, Rigidbody rb)
        {
            _playerInput = playerInput;
            _rb = rb;

            _playerInput.gameActions.Player.Attack.performed += TryAttack;
        }

        private void TryAttack(InputAction.CallbackContext context)
        {
            
        }

        private void LateUpdate()
        {
        }

        private void Aim(Vector3 dir)
        {

        }


        private void OnEnable()
        {
            if (_playerInput == null) return;
            
            _playerInput.gameActions.Player.Attack.performed += TryAttack;
        }

        private void OnDisable()
        {
            if (_playerInput == null) return;

            _playerInput.gameActions.Player.Attack.performed -= TryAttack;
        }
    }
}
