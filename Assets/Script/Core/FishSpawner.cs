using UnityEngine;

[System.Serializable]
public class OceanFishLevel
{
    public GameObject[] normalFish;
    public GameObject[] rareFish;
}
public class FishSpawner : MonoBehaviour
{
    [Header("Fish Berdasarkan Ocean Level")]
    public OceanFishLevel[] fishByLevel;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public int maxFishAlive = 20;
    [Range(0f, 1f)]
    public float rareFishChance = 0.2f;

    [Header("Spawn Area")]
    public Transform areaCenter;
    public Vector2 areaSize = new Vector2(10f, 5f);
    public Collider2D fishWaterArea;

    [Header("Obstacle Check")]
    public LayerMask obstacleLayer;
    public float spawnCheckRadius = 0.3f;
    public int maxSpawnAttempts = 30;

    [Header("Ocean Level")]
    public string oceanLevelKey = "oceanCleanUp";

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

        if (Physics2D.OverlapCircle(spawnPosition, spawnCheckRadius, obstacleLayer))
        {
            Debug.LogWarning("Tidak menemukan posisi spawn yang aman.");
            return;
        }
        GameObject fishObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Fish fishComponent = fishObject.GetComponent<Fish>();
        if (fishComponent != null && fishWaterArea != null)
        {
            fishComponent.waterArea = fishWaterArea;
        }
    }

    GameObject ChooseFishPrefab()
    {
        int oceanLevel = PlayerPrefs.GetInt(oceanLevelKey, 0);

        oceanLevel = Mathf.Clamp(
            oceanLevel,
            0,
            fishByLevel.Length - 1
        );

        OceanFishLevel levelFish = fishByLevel[oceanLevel];

        bool spawnRare =
            Random.value < rareFishChance &&
            levelFish.rareFish.Length > 0;

        if (spawnRare)
        {
            return levelFish.rareFish[
                Random.Range(0, levelFish.rareFish.Length)
            ];
        }

        if (levelFish.normalFish.Length > 0)
        {
            return levelFish.normalFish[
                Random.Range(0, levelFish.normalFish.Length)
            ];
        }

        if (levelFish.rareFish.Length > 0)
        {
            return levelFish.rareFish[
                Random.Range(0, levelFish.rareFish.Length)
            ];
        }

        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = areaCenter != null ? areaCenter.position : transform.position;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float x = center.x + Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
            float y = center.y + Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);

            Vector2 spawnPos = new Vector2(x, y);

            // Cek apakah posisi mengenai obstacle
            if (!Physics2D.OverlapCircle(spawnPos, spawnCheckRadius, obstacleLayer))
            {
                return new Vector3(x, y, center.z);
            }
        }

        // Kalau gagal setelah beberapa kali percobaan
        return center;
    }

    int CountActiveFish()
    {
        Fish[] activeFish = FindObjectsByType<Fish>();
        return activeFish.Length;
    }
}
