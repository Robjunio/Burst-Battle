using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private Rigidbody _rb;

        private float _lastAttackTime;

        private bool _isAttacking = false;

        private void Awake()
        {
            TryGetComponent(out _rb);
        }

        public void TryAttack(InputAction.CallbackContext context)
        {
            
        }

        private void LateUpdate()
        {

        }

        private void Aim(Vector3 dir)
        {

        }
    }
}
