using UnityEngine;
using TMPro;

public class OxygenUpgrade : MonoBehaviour
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
        currentLevel = PlayerPrefs.GetInt("OxygenLevel", 0);

        UpdateUI();
    }

    public void UpgradeOxygen()
    {
        int coins = PlayerPrefs.GetInt("CoinCount", 0);

        // max level?
        if (currentLevel >= upgradeCosts.Length)
        {
            Debug.Log("Upgrade MAX");
            return;
        }

        int cost = upgradeCosts[currentLevel];

        if (coins >= cost)
        {
            coins -= cost;

            PlayerPrefs.SetInt("CoinCount", coins);

            currentLevel++;

            PlayerPrefs.SetInt("OxygenLevel", currentLevel);

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
            levelText.text = "Oxygen Lv. " + currentLevel;
        }

        if (costText != null)
        {
            if (currentLevel >= upgradeCosts.Length)
            {
                costText.text = "MAX";
            }
            else
            {
                costText.text = "Cost : " + upgradeCosts[currentLevel];
            }
        }
    }
}
