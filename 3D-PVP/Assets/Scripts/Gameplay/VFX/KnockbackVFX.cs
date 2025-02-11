using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackVFX : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private PlayerController PlayerController;
    private Rigidbody PlayerRB;
    private Rigidbody HazardRB;
    private Vector3 moveDirection;
    [SerializeField] GameObject KnockBackVFX;
    [SerializeField] GameObject KnockBackDeathVFX;


    // Update is called once per frame
    void Start()
    {
        PlayerRB = PlayerPrefab.GetComponent<Rigidbody>();
        
    }


     void OnCollisionEnter(Collision collision)
     {
        if (collision.gameObject.CompareTag("Death"))
        {
           
            HazardRB = collision.gameObject.GetComponent<Rigidbody>();
            moveDirection = PlayerRB.transform.position - HazardRB.transform.position;
            PlayerRB.AddForce(moveDirection.normalized * 2000f);
            KnockBackVFX.SetActive(true);

            
        }

        if (collision.gameObject.CompareTag("Wall") && PlayerController.dead)
        {
            KnockBackDeathVFX.SetActive(true);
            KnockBackVFX.SetActive(false);
        }
    }
   
}
