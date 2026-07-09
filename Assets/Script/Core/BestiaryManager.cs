using System.Collections.Generic;
using UnityEngine;

public class BestiaryManager : MonoBehaviour
{
    public static BestiaryManager instance;

    private HashSet<string> discoveredFish = new HashSet<string>();

    public List<FishData> allFish;

    void Awake()
    {
        // cegah duplicate manager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadFish(allFish);
    }

    public void RegisterFish(FishData fish)
    {
        if (!discoveredFish.Contains(fish.fishID))
        {
            discoveredFish.Add(fish.fishID);

            SaveFish(fish.fishID);
            if (BestiaryUI.instance != null)
            {
                BestiaryUI.instance.RefreshUI();
            }

            Debug.Log(
                "Bestiary baru: " + fish.fishName
            );
        }
    }

    public bool IsDiscovered(FishData fish)
    {
        return discoveredFish.Contains(fish.fishID);
    }

    void SaveFish(string fishID)
    {
        PlayerPrefs.SetInt("Fish_" + fishID, 1);

        PlayerPrefs.Save();
    }

    void LoadFish(List<FishData> fishList)
    {
        foreach (FishData fish in fishList)
        {
            if (
                PlayerPrefs.GetInt(
                    "Fish_" + fish.fishID,
                    0
                ) == 1
            )
            {
                discoveredFish.Add(fish.fishID);
            }
        }
    }

    public List<string> GetDiscoveredFishNames()
    {
    List<string> names = new();

        foreach (FishData fish in allFish)
        {
            if (IsDiscovered(fish))
            {
                names.Add(fish.fishName);
            }
        }

    return names;

    }

}