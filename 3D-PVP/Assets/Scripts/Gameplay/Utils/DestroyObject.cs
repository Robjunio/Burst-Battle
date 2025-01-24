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

    private void OnEnable()
    {
        EventManager.EndMatch += DestroyObj;
    }

    private void OnDisable()
    {
        EventManager.EndMatch -= DestroyObj;
    }
}
