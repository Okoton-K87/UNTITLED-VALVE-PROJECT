using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject zombiePrefab; // Reference to the zombie prefab
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnRate = 2f; // Time between spawns
    public float spawnRadius = 10f; // Radius around the spawner to spawn zombies

    [Header("Player Detection")]
    public Transform playerTransform; // Reference to the player's transform
    public float detectionRadius = 20f; // Radius to check if the player is in the area

    private bool isPlayerInArea = false; // Is the player in the spawn area?
    private float spawnTimer; // Timer for spawning

    private void Update()
    {
        // Check if the player is within the detection radius
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            if (!isPlayerInArea)
            {
                isPlayerInArea = true;
                Debug.Log("Player entered the spawn area.");
            }
            HandleZombieSpawning();
        }
        else
        {
            if (isPlayerInArea)
            {
                isPlayerInArea = false;
                Debug.Log("Player exited the spawn area.");
            }
        }
    }

    private void HandleZombieSpawning()
    {
        spawnTimer += Time.deltaTime;

        // Spawn zombies at a regular interval
        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        // Choose a random spawn point
        Vector3 spawnPosition;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            spawnPosition = spawnPoints[randomIndex].position;
        }
        else
        {
            // Randomize a position within the radius if no specific spawn points are set
            spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // Keep the y-coordinate the same as the spawner
        }

        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Zombie spawned at: " + spawnPosition);
    }
}
