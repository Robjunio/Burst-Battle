using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BathBomb : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject bombVfx;
    private Vector3 bombPos;
    bool exploded = false;
    

    private void Start()
    {        
        transform.GetChild(0).name = gameObject.name;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded) return;
        StartCoroutine(Explode());
        bombPos = bomb.transform.position;
        SpawnParticle("BubbleExplosionParticle", bombPos);
        ReturnParticleToPool(bombVfx);


    }

    IEnumerator Explode()
    {
        exploded = true;
        Camera.main.transform.DOShakePosition(0.5f);

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/445961__breviceps__cartoon-video-game-bubble-shot"), Camera.main.transform.position);
        transform.GetChild(0).gameObject.SetActive(true);

        yield return null;
        
        gameObject.SetActive(false);
    }

    private void SpawnParticle(string prefabName, Vector3 position)
    {
        // Call the ParticleSysManager's GetParticle method
        GameObject particle = ParticleSysManager.Instance.GetParticle(prefabName, position);

        if (particle != null)
        {
            Debug.Log($"Spawned {prefabName} at {position}");
        }
        else
        {
            Debug.LogWarning($"Failed to spawn {prefabName}. Pool might be empty.");
            
        }
        
    }

    private void ReturnParticleToPool(GameObject particle)
    {
        if (particle != null)
        {
            ParticleSysManager.Instance.ReturnParticle(particle);
            Debug.Log($"Returned {particle.name} to the pool.");
        }
        else
        {
            Debug.LogWarning("No particle assigned to return.");
        }
    }
}
