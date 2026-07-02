using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;

    void OnEnable()
    {
        if (Inventory.instance != null)
        {
            UpdateInventoryUI(Inventory.instance.fishCount);
        }
    }

    public void UpdateInventoryUI(int fishCount)
    {
        inventoryText.text = "Total ikan: " + fishCount + " / 10";
    }

}
