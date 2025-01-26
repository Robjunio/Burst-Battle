using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/246283__afleetingspeck__g-guitar-chord-hit-percussion"), Camera.main.transform.position);
    }
}
