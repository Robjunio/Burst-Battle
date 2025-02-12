using UnityEngine;
using Unity.Netcode;

public class DestroyNetcodeObject : NetworkBehaviour
{
    [SerializeField] private float time;

    void Start()
    {
        Invoke("DestroyObj", time);
    }

    void DestroyObj()
    {
        if (!NetworkObject.IsSpawned && !IsServer) return;
        NetworkObject.Despawn(true);
    }

    private void OnEnable()
    {
        EventManager.EndMatch += DestroyObj;
    }

    private void OnDisable()
    {
        EventManager.EndMatch -= DestroyObj;
    }
}
