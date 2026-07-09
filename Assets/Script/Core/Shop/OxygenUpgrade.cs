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
        currentLevel =
            PlayerPrefs.GetInt(
                "OxygenLevel",
                0
            );

        UpdateUI();
    }

    public void UpgradeOxygen()
    {
        // MAX LEVEL?
        if (
            currentLevel >=
            upgradeCosts.Length
        )
        {
            Debug.Log("Upgrade MAX");

            return;
        }

        int cost =
            upgradeCosts[currentLevel];

        // AMBIL COIN DARI COIN MANAGER
        int coins =
            CoinManager.instance.coins;

        if (coins >= cost)
        {
            // KURANGI COIN
            CoinManager.instance.RemoveCoins(cost);

            currentLevel++;

            PlayerPrefs.SetInt(
                "OxygenLevel",
                currentLevel
            );

            PlayerPrefs.Save();

            UpdateUI();

            Debug.Log(
                "Upgrade berhasil"
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
                "Oxygen Lv. " +
                currentLevel;
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