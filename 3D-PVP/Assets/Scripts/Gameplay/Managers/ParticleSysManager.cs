using System.Collections.Generic;
using UnityEngine;

public class ParticleSysManager : MonoBehaviour
{
    public static ParticleSysManager Instance; // Singleton for easy access

    // List of particle prefabs
    public List<GameObject> particlePrefabs;

    public int poolSize = 5; // Number of particles to pool per type

    // Dictionary to store pools for each particle type
    private Dictionary<string, Queue<GameObject>> particlePools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializePools();
    }

    // Initialize pools for all particle types
    private void InitializePools()
    {
        foreach (GameObject prefab in particlePrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject particle = Instantiate(prefab, transform);
                particle.SetActive(false);
                pool.Enqueue(particle);
            }

            // Use the prefab name as the key for the pool
            particlePools.Add(prefab.name, pool);
        }
    }

    // Get a particle from the pool by prefab name
    public GameObject GetParticle(string prefabName, Vector3 position)
    {
        if (!particlePools.ContainsKey(prefabName))
        {
            Debug.LogError($"Particle prefab '{prefabName}' not found in pool!");
            return null;
        }

        if (particlePools[prefabName].Count == 0)
        {
            Debug.LogWarning($"Pool for '{prefabName}' is empty! Consider increasing pool size.");
            return null;
        }

        GameObject particle = particlePools[prefabName].Dequeue();
        particle.transform.position = position;
        particle.SetActive(true);
        return particle;
    }

    // Return a particle to the pool
    public void ReturnParticle(GameObject particle)
    {
        string prefabName = particle.name.Replace("(Clone)", ""); // Remove "(Clone)" from the name
        if (!particlePools.ContainsKey(prefabName))
        {
            Debug.LogError($"Particle prefab '{prefabName}' not found in pool!");
            return;
        }

        particle.SetActive(false);
        particlePools[prefabName].Enqueue(particle);
    }
}