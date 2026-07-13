using UnityEngine;
using TMPro;

public class OceanCleanUP : MonoBehaviour
{
    [Header("Upgrade Data")]
    public int[] upgradeCosts =
    {
        10,
        25,
        50,
        100
    };

    [Header("UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;

    int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("oceanCleanUp", 0);
        UpdateUI();
    }

    public void UpgradeOceanCleanUp()
    {
        if (currentLevel >= upgradeCosts.Length)
        {
            Debug.Log("Upgrade MAX");
            return;
        }

        int cost = upgradeCosts[currentLevel];
        int coins = CoinManager.instance.coins;

        if (coins >= cost)
        {
            CoinManager.instance.RemoveCoins(cost);

            currentLevel++;

            PlayerPrefs.SetInt("oceanCleanUp", currentLevel);
            PlayerPrefs.Save();

            UpdateUI();

            Debug.Log("Upgrade berhasil");
        }
        else
        {
            Debug.Log("Coin tidak cukup");
        }
    }

    void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Ocean CleanUp Lv. " + currentLevel;
        }

        if (costText != null)
        {
            if (currentLevel >= upgradeCosts.Length)
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
