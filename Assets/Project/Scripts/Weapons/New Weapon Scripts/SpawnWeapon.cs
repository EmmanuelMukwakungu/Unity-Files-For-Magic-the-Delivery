using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnWeapon : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject[] weaponPrefabs; // List of spawnable weapons

    [Header("Spawn Settings")]
    public float respawnDelay = 5f;        // Delay before respawning weapon after pickup
    public float spawnRadius = 0f;         // Optional offset for spawn randomness
    public float spawnDelayAfterEnter = 2f; // Delay after player enters before spawn

    private GameObject currentWeapon;
    private bool waitingToRespawn = false;
    private bool playerInside = false;

    private void Update()
    {
        // Only respawn when player is inside, no weapon exists, and not already waiting
        if (playerInside && currentWeapon == null && !waitingToRespawn)
        {
            waitingToRespawn = true;
            Invoke(nameof(SpawnRandomWeapon), respawnDelay);
        }
    }

    private void SpawnRandomWeapon()
    {
        // Prevent double-spawning if weapon already exists
        if (currentWeapon != null) return;

        if (weaponPrefabs.Length == 0)
        {
            Debug.LogWarning("No weapon prefabs assigned to SpawnWeapon!");
            return;
        }

        int randomIndex = Random.Range(0, weaponPrefabs.Length);
        GameObject randomWeapon = weaponPrefabs[randomIndex];

        Vector3 spawnPos = transform.position;
        if (spawnRadius > 0f)
        {
            spawnPos += new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );
        }

        currentWeapon = Instantiate(randomWeapon, spawnPos, Quaternion.identity);
        waitingToRespawn = false;

        // Listen for when weapon is picked up
        WeaponPickUp pickUpScript = currentWeapon.GetComponent<WeaponPickUp>();
        if (pickUpScript != null)
        {
            pickUpScript.OnWeaponPickedUp += HandleWeaponPickedUp;
        }

        Debug.Log($"Spawned: {randomWeapon.name}");
    }

    private void HandleWeaponPickedUp()
    {
        // Weapon was picked up — clear reference so new one can spawn later
        currentWeapon = null;
        waitingToRespawn = false;
        Debug.Log("Weapon picked up — ready to respawn after delay.");
    }

   

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelayAfterEnter);
        if (playerInside && currentWeapon == null)
            SpawnRandomWeapon();
    }
}
