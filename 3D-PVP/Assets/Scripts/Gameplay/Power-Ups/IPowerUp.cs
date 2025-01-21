using UnityEngine;

public interface IPowerUp 
{
    void UsePowerUp(Transform SpawnPosition, Vector2 dir);
    void DestroyPowerUp();
    bool CheckDurability();
}
