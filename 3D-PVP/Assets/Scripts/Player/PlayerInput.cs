using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public GameActions gameActions { get; private set; }
        void Awake()
        {
            gameActions = new GameActions();
        }

        public Vector3 GetMovement()
        {
            Vector2 dir = gameActions.Player.Move.ReadValue<Vector2>();

            return new Vector3(dir.x, 0, dir.y);
        }
        private void OnEnable()
        {
            gameActions.Player.Enable();
        }

        private void OnDisable()
        {
            gameActions.Player.Disable();
        }
    }
}
