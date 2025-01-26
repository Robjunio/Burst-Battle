using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject[] _powerUps;

    public Animator GetAnimator() { return _animator; }

    public void ResetPowerUps()
    {
        foreach(var powerUp in _powerUps)
        {
            powerUp.SetActive(false);
        }
    }

    public void SetActivePowerUp(int id)
    {
        _powerUps[id].SetActive(true);
    }
}
