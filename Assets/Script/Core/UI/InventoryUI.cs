using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;

    public void UpdateInventoryUI(int fishCount)
    {
        inventoryText.text = "total ikan: " + fishCount.ToString() + " / 10";
    }
}
