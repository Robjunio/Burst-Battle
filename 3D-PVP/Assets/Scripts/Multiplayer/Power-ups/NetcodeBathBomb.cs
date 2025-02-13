using DG.Tweening;
using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System;

public class NetcodeBathBomb : NetworkBehaviour
{
    GameObject bomb;
    bool exploded = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        bomb = Resources.Load<GameObject>("Prefabs/BubbleExplosionParticle");
        transform.GetChild(0).name = gameObject.name;
        transform.GetChild(0).gameObject.SetActive(false);
        exploded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded || !IsServer) return;

        ExplodeClientRpc();
    }

    [ClientRpc]
    private void ExplodeClientRpc()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        Instantiate(bomb, transform.position, Quaternion.identity);

        exploded = true;
        Camera.main.transform.DOShakePosition(0.5f);

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/445961__breviceps__cartoon-video-game-bubble-shot"), Camera.main.transform.position);
        transform.GetChild(0).gameObject.SetActive(true);

        yield return null;

        if (NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }
    }
}
