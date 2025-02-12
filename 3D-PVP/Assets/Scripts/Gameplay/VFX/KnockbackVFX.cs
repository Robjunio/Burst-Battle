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
        EventManager.StartMatch += EnableRBConstrains;
        EventManager.EndMatch += DiableEffect;
    }

    private void OnDestroy()
    {
        EventManager.EndMatch -= DiableEffect;
        EventManager.StartMatch -= EnableRBConstrains;
    }


     void OnCollisionEnter(Collision collision)
     {
        if (collision.gameObject.CompareTag("Death") && collision.gameObject.GetComponent<Rigidbody>() != null)
        {
           
            HazardRB = collision.gameObject.GetComponent<Rigidbody>();
            moveDirection = PlayerRB.transform.position - HazardRB.transform.position;
            PlayerRB.AddForce(moveDirection.normalized * 1500f);
            KnockBackVFX.SetActive(true);
            PlayerPrefab.layer = LayerMask.NameToLayer("DeadLayer");


        }

        if (collision.gameObject.CompareTag("Wall") && PlayerController.dead)
        {
            KnockBackDeathVFX.SetActive(true);
            
            PlayerRB.constraints = RigidbodyConstraints.FreezeAll;
        }
     }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Death") && collider.gameObject.GetComponent<Rigidbody>() != null)
        {

            HazardRB = collider.gameObject.GetComponent<Rigidbody>();
            moveDirection = PlayerRB.transform.position - HazardRB.transform.position;
            PlayerRB.AddForce(moveDirection.normalized * 1500f);
            KnockBackVFX.SetActive(true);
            PlayerPrefab.layer = LayerMask.NameToLayer("DeadLayer");


        }

        if (collider.gameObject.CompareTag("Wall") && PlayerController.dead)
        {
            KnockBackDeathVFX.SetActive(true);
            
            PlayerRB.constraints = RigidbodyConstraints.FreezeAll;


        }
    }

    private void DiableEffect()
    {
        
        KnockBackDeathVFX.SetActive(false);
        KnockBackVFX.SetActive(false);
        
    }

    private void EnableRBConstrains()
    {
        PlayerPrefab.layer = LayerMask.NameToLayer("Default");
        PlayerRB.constraints = RigidbodyConstraints.None;
        PlayerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    

}
