using UnityEngine;
using UnityEngine.UI;

public class OceanCleanUpUI : MonoBehaviour
{
    [Header("Objects berdasarkan Level Upgrade")]
    public GameObject[] levelObjects;


    [Header("Progress")]
    public Slider progressBar;
    public int totalUpgradeLevels = 5;

    [Header("PlayerPrefs Key")]
    public string playerPrefsKey = "oceanCleanUp";

    private void Start()
    {
        UpdateLevelObject();
    }

    public void UpdateLevelObject()
    {
        int currentLevel = PlayerPrefs.GetInt(playerPrefsKey, 0);
        int maxLevel = Mathf.Max(totalUpgradeLevels, 1);

        // Mencegah index melebihi jumlah object
        currentLevel = Mathf.Clamp(currentLevel, 0, Mathf.Max(levelObjects.Length - 1, 0));

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (levelObjects[i] != null)
            {
                levelObjects[i].SetActive(i == currentLevel);
            }
        }

        if (progressBar != null)
        {
            progressBar.value = currentLevel / (float)maxLevel;
        }
    }
}