using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

namespace Player
{
    public class NetcodePlayerAttack : NetworkBehaviour
    {
        public bool alive;

        [SerializeField] Transform attackSpawn;
        private IPowerUp powerUpEquipped;

        private Vector3 _aim = Vector3.forward;

        private PlayerSkin playerSkin;

        private GameActions gameActions;
        private NetcodePlayerMovement netcodePlayerMovement;

        public void SetInput(GameActions gameActions, NetcodePlayerMovement playerMovement)
        {
            this.gameActions = gameActions;
            netcodePlayerMovement = playerMovement;
            gameActions.Player.Attack.performed += TryAttack;
        }

        private void Update()
        {
            GetAim();    
        }

        public void GetAim()
        {
            if (!alive) return;
            var dir = netcodePlayerMovement.GetInput();
            if (dir != Vector3.zero) 
            {
                _aim = dir;
            }
        }

        public void TryAttack(InputAction.CallbackContext ctx)
        {
            if (!alive && IsOwner) return;
            if (powerUpEquipped != null)
            {
                UsePowerUpServerRpc();
            }
        }

        [ServerRpc]
        private void UsePowerUpServerRpc()
        {
            powerUpEquipped.UsePowerUp(attackSpawn, _aim, gameObject.name);
            if (!powerUpEquipped.CheckDurability())
            {
                RemovePowerUpClientRpc();
            }
        }

        [ClientRpc]
        private void RemovePowerUpClientRpc()
        {
            powerUpEquipped.DestroyPowerUp();
            playerSkin.ResetPowerUps();

            powerUpEquipped = null;
        }

        public void SetPowerUp(IPowerUp power)
        {
            if (powerUpEquipped == null)
            {
                powerUpEquipped = power;
                playerSkin.SetActivePowerUp(power.GetPowerUpID());
            }
        }

        public void ResetPowerUp()
        {
            powerUpEquipped = null;
            playerSkin.ResetPowerUps();

            _aim = Vector3.forward;
        }

        public void SetPlayerSkin(PlayerSkin player)
        {
            playerSkin = player;
        }
    }
}
