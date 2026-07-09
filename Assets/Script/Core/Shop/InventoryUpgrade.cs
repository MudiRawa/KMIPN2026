using UnityEngine;
using TMPro;

public class InventoryUpgrade : MonoBehaviour
{
    [Header("Upgrade Cost")]
    public int[] upgradeCosts =
    {
        15,
        30,
        60,
        120
    };

    [Header("Inventory Size")]
    public int[] inventorySizes =
    {
        10,
        15,
        20,
        30,
        40
    };

    [Header("UI")]
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI capacityText;

    public TextMeshProUGUI costText;

    int currentLevel;

    void Start()
    {
        currentLevel =
            PlayerPrefs.GetInt(
                "InventoryLevel",
                0
            );

        UpdateUI();
    }

    public void UpgradeInventory()
    {
        if (
            currentLevel >=
            upgradeCosts.Length
        )
        {
            Debug.Log("Inventory MAX");

            return;
        }

        int cost =
            upgradeCosts[currentLevel];

        if (
            CoinManager.instance.coins >= cost
        )
        {
            CoinManager.instance.RemoveCoins(cost);

            currentLevel++;

            PlayerPrefs.SetInt(
                "InventoryLevel",
                currentLevel
            );

            PlayerPrefs.Save();

            // langsung update inventory yang sedang dipakai
            Inventory.instance.maxFish =
                inventorySizes[currentLevel];

            Inventory.instance.RefreshUI();

            UpdateUI();

            Debug.Log(
                "Inventory berhasil diupgrade"
            );
        }
        else
        {
            Debug.Log(
                "Coin tidak cukup"
            );
        }
    }

    void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text =
                "Inventory Lv. " +
                currentLevel;
        }

        if (capacityText != null)
        {
            capacityText.text =
                inventorySizes[currentLevel] +
                " Fish";
        }

        if (costText != null)
        {
            if (
                currentLevel >=
                upgradeCosts.Length
            )
            {
                costText.text = "MAX";
            }
            else
            {
                costText.text = upgradeCosts[currentLevel].ToString();
            }
        }
    }
}