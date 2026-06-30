using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int fishCount = 0;
    public int maxFish = 10;

    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        fishCount = PlayerPrefs.GetInt("FishCount", 0);
        inventoryUI.UpdateInventoryUI(fishCount);
    }

    public void AddFish()
    {
        if (fishCount < maxFish)
        {
            fishCount+= 1;
            inventoryUI.UpdateInventoryUI(fishCount);
            Debug.Log("Ikan ditambahkan ke inventaris. Total ikan: " + fishCount + " / " + maxFish);
            PlayerPrefs.SetInt("FishCount", fishCount);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Inventaris penuh. Ikan tidak dapat ditambahkan.");
        }
    }
}
