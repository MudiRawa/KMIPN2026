using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("Inventory")]
    public int fishCount = 0;
    public int maxFish = 10;

    public List<FishInventoryItem> fishes = new List<FishInventoryItem>();

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

    void Start()
    {
        LoadInventory();
    }

    // TAMBAH IKAN
    public void AddFish(FishData fish)
    {
        if (fishCount >= maxFish)
        {
            Debug.Log(
                "Inventory penuh!"
            );

            return;
        }

        fishCount++;

        FishInventoryItem item = new FishInventoryItem();

        item.fishData = fish;
        fishes.Add(item);
        SaveInventory();
        RefreshUI();
        Debug.Log(fish.fishName + " ditambahkan");
    }

    // HAPUS IKAN
    public void RemoveFish(FishInventoryItem fish)
    {
        fishes.Remove(fish);
        fishCount--;
        SaveInventory();
        RefreshUI();
    }

    // UPDATE UI
    public void RefreshUI()
    {
        InventoryUI ui =
            FindAnyObjectByType<InventoryUI>();

        if (ui != null)
        {
            ui.UpdateInventoryUI(
                fishCount
            );
        }
    }

    // SAVE
    void SaveInventory()
    {
        List<string> ids =
            new List<string>();

        foreach (
            FishInventoryItem fish
            in fishes
        )
        {
            ids.Add(
                fish.fishData.fishID
            );
        }

        string saveString =
            string.Join(",", ids);

        PlayerPrefs.SetString(
            "Inventory",
            saveString
        );

        PlayerPrefs.SetInt(
            "FishCount",
            fishCount
        );

        PlayerPrefs.Save();
    }

    // LOAD
    void LoadInventory()
    {
        fishCount =
            PlayerPrefs.GetInt(
                "FishCount",
                0
            );

        string saveString =
            PlayerPrefs.GetString(
                "Inventory",
                ""
            );

        if (saveString == "")
            return;

        string[] ids =
            saveString.Split(',');

        foreach (string id in ids)
        {
            FishData fish =
                FishDatabase.instance
                .GetFishByID(id);

            if (fish != null)
            {
                FishInventoryItem item =
                    new FishInventoryItem();

                item.fishData = fish;

                fishes.Add(item);
            }
        }
    }

}
