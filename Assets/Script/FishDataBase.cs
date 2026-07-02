using System.Collections.Generic;
using UnityEngine;

public class FishDatabase : MonoBehaviour
{
    public static FishDatabase instance;

    public List<FishData> allFish;

    void Awake()
    {
        // singleton
        if (instance != null &&
            instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // jangan hancur saat pindah scene
        DontDestroyOnLoad(gameObject);
    }

    public FishData GetFishByID(string id)
    {
        foreach (FishData fish in allFish)
        {
            if (fish.fishID == id)
            {
                Debug.Log("Ketemu fish: " + id);
                return fish;
            }
        }
        Debug.Log("Fish TIDAK ketemu: " + id);
        return null;

    }


}
