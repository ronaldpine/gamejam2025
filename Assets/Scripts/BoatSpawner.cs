using UnityEngine;

public class BoatSpawner : MonoBehaviour
{
    [Header("Boat Settings")]
    public GameObject boatPrefab;
    public Transform islandCenter;

    [Header("Map Settings")]
    public int mapWidth = 10;
    public int mapHeight = 10;
    public float tileWidth = 1f;
    public float tileHeight = 0.5f;

    [Header("Spawn Ring")]
    public int spawnRingStart = 10;  // How far from center boats can start
    public int spawnRingEnd = 12;    // Max distance

    [Header("Timing")]
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBoat), 2f, spawnInterval);
    }

    void SpawnBoat()
    {
        // 1. Pick a random ring distance and angle
        int dist = Random.Range(spawnRingStart, spawnRingEnd + 1);
        float angleRad = Random.Range(0f, 2f * Mathf.PI);

        float x = Mathf.Cos(angleRad) * dist;
        float y = Mathf.Sin(angleRad) * dist;

        // 2. Convert to isometric space
        float isoX = (x - y) * (tileWidth / 2f);
        float isoY = (x + y) * (tileHeight / 2f);

        Vector3 spawnPosition = new Vector3(isoX, isoY, 0f);

        // 3. Instantiate boat without rotation

        GameObject boat = Instantiate(boatPrefab, spawnPosition, Quaternion.identity);

        // Calculate direction and angle to the island center
        Vector3 direction = (islandCenter.position - spawnPosition).normalized;
        float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate to face the island (+180° correction)
        angleDeg += 180f;

        // Apply rotation
        boat.transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);

        // Flip vertically if needed to keep visual uprightness
        if (angleDeg > 90f && angleDeg < 270f)
        {
            Vector3 scale = boat.transform.localScale;
            scale.y = -Mathf.Abs(scale.y); // Flip vertically
            boat.transform.localScale = scale;
        }
        else
        {
            Vector3 scale = boat.transform.localScale;
            scale.y = Mathf.Abs(scale.y); // Keep upright
            boat.transform.localScale = scale;
        }



    }
}


