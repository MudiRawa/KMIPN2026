using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Fish Prefabs")]
    public GameObject[] normalFishPrefabs;
    public GameObject[] rareFishPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public int maxFishAlive = 20;
    [Range(0f, 1f)]
    public float rareFishChance = 0.2f;

    [Header("Spawn Area")]
    public Transform areaCenter;
    public Vector2 areaSize = new Vector2(10f, 5f);
    public Collider2D fishWaterArea;

    float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        if (CountActiveFish() >= maxFishAlive)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnFish();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnFish()
    {
        GameObject prefab = ChooseFishPrefab();
        if (prefab == null)
        {
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject fishObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Fish fishComponent = fishObject.GetComponent<Fish>();
        if (fishComponent != null && fishWaterArea != null)
        {
            fishComponent.waterArea = fishWaterArea;
        }
    }

    GameObject ChooseFishPrefab()
    {
        bool spawnRare = Random.value < rareFishChance && rareFishPrefabs.Length > 0;

        if (spawnRare)
        {
            return rareFishPrefabs[Random.Range(0, rareFishPrefabs.Length)];
        }

        if (normalFishPrefabs.Length > 0)
        {
            return normalFishPrefabs[Random.Range(0, normalFishPrefabs.Length)];
        }

        if (rareFishPrefabs.Length > 0)
        {
            return rareFishPrefabs[Random.Range(0, rareFishPrefabs.Length)];
        }

        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = areaCenter != null ? areaCenter.position : transform.position;
        float x = center.x + Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
        float y = center.y + Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);
        return new Vector3(x, y, center.z);
    }

    int CountActiveFish()
    {
        Fish[] activeFish = FindObjectsByType<Fish>();
        return activeFish.Length;
    }
}
