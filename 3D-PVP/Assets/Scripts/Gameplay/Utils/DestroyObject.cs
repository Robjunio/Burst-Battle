using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float time;

    void Start()
    {
        Invoke("DestroyObj", time);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
