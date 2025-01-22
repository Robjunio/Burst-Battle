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
        transform.GetChild(0).gameObject.SetActive(true);
        yield return null;
        
        gameObject.SetActive(false);
    }
}
