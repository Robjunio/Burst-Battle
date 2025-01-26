using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BathBomb : MonoBehaviour
{
    GameObject bomb;
    bool exploded = false;

    private void Start()
    {
        bomb = Resources.Load<GameObject>("Prefabs/BubbleExplosionParticle");
        transform.GetChild(0).name = gameObject.name;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded) return;
        StartCoroutine(Explode());
        Instantiate(bomb, transform.position, Quaternion.identity);
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
}
