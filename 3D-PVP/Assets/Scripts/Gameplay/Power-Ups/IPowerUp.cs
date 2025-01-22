using UnityEngine;

public interface IPowerUp 
{
    void UsePowerUp(Transform SpawnPosition, Vector3 dir);
    void DestroyPowerUp();
    bool CheckDurability();

    void ResetDurability();
}
