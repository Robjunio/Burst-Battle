using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackVFXHandler: MonoBehaviour
{
    [SerializeField] GameObject KnockBackVFX;
    [SerializeField] GameObject KnockBackDeathVFX;
    bool exploded;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.EndMatch += DiableEffect;
    }

    private void OnDestroy()
    {
        EventManager.EndMatch -= DiableEffect;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !exploded)
        {
            exploded = true;
            KnockBackVFX.SetActive(false);
            KnockBackDeathVFX.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void DiableEffect()
    {
        KnockBackDeathVFX.SetActive(false);
        KnockBackVFX.SetActive(false);
    }
}
