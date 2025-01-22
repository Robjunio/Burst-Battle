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
