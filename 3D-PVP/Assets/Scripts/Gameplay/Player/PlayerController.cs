using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        var list = FindObjectsOfType<PlayerController>();
        gameObject.name = "Player " + list.Length.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            this.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Bubble"))
        {
            HandlerBubbleAttack();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void HandlerBubbleAttack()
    {
        if(transform.localScale == new Vector3(2, 2, 2))
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }
}
