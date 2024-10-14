using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab; //Script do objeto Asteroid
    public EnemyShip enemyShipPrefab; //Script do objeto EnemyShip
    public float spawnDistance = 12f;
    
    public float spawnRate = 1f;
    public int amountPerSpawn = 1;
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;
    [SerializeField] public int maxEnemiesAlive;

    public Transform shipATransform;

    private void Start()
    {
        if (shipATransform == null)
        {
            Debug.LogError("ShipA Transform is not assigned to the AsteroidSpawner!");
            return;
        }
        Invoke("Spawner", 1f);
    }

    public void Spawner()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    public void Spawn()
    {
        // Check the number of objects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length >= maxEnemiesAlive)
        {
            return;
        }

        for (int i = 0; i < amountPerSpawn; i++)
        {
            // Randomly determine the type of object to spawn
            float randomValue = Random.value; // Random value between 0 and 1

            if (randomValue <= 0.3f) // 30% chance of spawning an Enemy Ship
            {
                // Spawn an Enemy Ship
                Vector2 spawnDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPoint = spawnDirection * spawnDistance;
                spawnPoint += shipATransform.position;
                float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
                EnemyShip enemyShip = Instantiate(enemyShipPrefab, spawnPoint, rotation);
                enemyShip.size = Random.Range(enemyShip.minSize, enemyShip.maxSize);
                Vector2 trajectory = rotation * -spawnDirection;
                //enemyShip.SetTrajectory(trajectory);
            }
            else // 70% chance of spawning an asteroid
            {
                // Spawn an Asteroid
                Vector2 spawnDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPoint = spawnDirection * spawnDistance;
                spawnPoint += shipATransform.position;
                float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
                Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
                asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
                Vector2 trajectory = rotation * -spawnDirection;
                asteroid.SetTrajectory(trajectory);
            }
        }
    }
}
