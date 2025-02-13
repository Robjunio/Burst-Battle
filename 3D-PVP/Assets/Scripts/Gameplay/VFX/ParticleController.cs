using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        // Play the particle system when enabled
        particleSystem.Play();
    }

    private void Update()
    {
        // Return to pool when the particle system finishes playing
        if (!particleSystem.IsAlive())
        {
            ParticleSysManager.Instance.ReturnParticle(gameObject);
        }
    }
}