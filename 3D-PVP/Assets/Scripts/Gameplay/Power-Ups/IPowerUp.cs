using UnityEngine;

public interface IPowerUp 
{
    void UsePowerUp(Transform SpawnPosition, Vector3 dir, string player);
    void DestroyPowerUp();
    bool CheckDurability();

    void ResetDurability();
}
