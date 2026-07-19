using UnityEngine;

public class BackgroundFishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs;

    public float spawnInterval = 3f;

    public Vector2 areaSize = new Vector2(30,15);

    public Transform center;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFish),1,spawnInterval);
    }

    void SpawnFish()
    {
        bool leftToRight = Random.value > 0.5f;

        Vector3 spawnPos;

        if(leftToRight)
        {
            spawnPos = new Vector3(
                center.position.x - areaSize.x/2,
                center.position.y + Random.Range(-areaSize.y/2,areaSize.y/2),
                22
            );
        }
        else
        {
            spawnPos = new Vector3(
                center.position.x + areaSize.x/2,
                center.position.y + Random.Range(-areaSize.y/2,areaSize.y/2),
                22
            );
        }

        GameObject fish = Instantiate(
            fishPrefabs[Random.Range(0,fishPrefabs.Length)],
            spawnPos,
            Quaternion.identity
        );

        Vector2 dir = leftToRight ? Vector2.right : Vector2.left;

        fish.GetComponent<BackgroundFish>().Initialize(dir);

        Destroy(fish,40f);
    }
}