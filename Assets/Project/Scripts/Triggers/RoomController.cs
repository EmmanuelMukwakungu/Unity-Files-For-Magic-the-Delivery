using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Room Doors")]
    public Collider entranceDoor; // assigned in inspector
    public Collider exitDoor;     // assigned in inspector

    [Header("Enemy Spawning")]
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPoints;
    public int enemiesPerWave = 3;
    
    [Header("Weapon Spawner")]
    public SpawnWeapon weaponSpawner;

    private int enemiesAlive = 0;
    private bool roomActive = false;

    void Start()
    {
        // Make sure player can walk through doors before fight
        entranceDoor.isTrigger = true;
        exitDoor.isTrigger = true;
    }

    public void PlayerEnteredRoom()
    {
        if (roomActive) return;
        roomActive = true;

        Debug.Log("Player entered room → locking doors!");

        // Lock doors
        entranceDoor.isTrigger = false;
        exitDoor.isTrigger = false;
        
        if (weaponSpawner != null)
            weaponSpawner.StartSpawning();

        // Spawn first wave
        SpawnWave();
    }

    private void SpawnWave()
    {
        enemiesAlive = enemiesPerWave;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform spawn = enemySpawnPoints[i % enemySpawnPoints.Length];
            GameObject enemy = Instantiate(enemyPrefab, spawn.position, spawn.rotation);

            // Hook event
            enemy.GetComponent<EnemyHealth>().roomController = this;
        }

        Debug.Log("Spawned wave with: " + enemiesPerWave);
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            Debug.Log("All enemies dead!");
            
            if (weaponSpawner != null)
                weaponSpawner.StopSpawning();

            // Open doors -> triggers again
            entranceDoor.isTrigger = true;
            exitDoor.isTrigger = true;

            roomActive = false;
        }
    }
}