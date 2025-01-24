using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathBomb : MonoBehaviour
{
    bool exploded = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (exploded) return;
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        exploded = true;
        Camera.main.transform.DOShakePosition(0.5f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return null;
        
        gameObject.SetActive(false);
    }
}
