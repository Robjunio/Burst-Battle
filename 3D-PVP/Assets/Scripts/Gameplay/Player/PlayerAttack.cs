using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] Transform attackSpawn;
        private IPowerUp powerUpEquipped;

        private Vector3 _aim = Vector3.forward;

        public void GetAim(InputAction.CallbackContext ctx)
        {
            Vector2 dir = ctx.ReadValue<Vector2>();

            if (dir == Vector2.zero) return;

            _aim = new Vector3(dir.x, 0, dir.y);
        }

        public void TryAttack(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Started)
            {
                if (powerUpEquipped != null)
                {
                    powerUpEquipped.UsePowerUp(attackSpawn, _aim);
                    if (!powerUpEquipped.CheckDurability())
                    {
                        powerUpEquipped.DestroyPowerUp();

                        powerUpEquipped = null;
                    }
                }
            }
        }

        public void SetPowerUp(IPowerUp power)
        {
            if (powerUpEquipped == null)
            {
                powerUpEquipped = power;
            }
        }
    }
}
