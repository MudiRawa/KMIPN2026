using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public FishData fishData;

    public Image icon;
    public TMP_Text priceText;

    public GameObject selectedMark;
    public FishInventoryItem fishItem;

    public bool selected = false;

    public void Setup(FishInventoryItem item)
    {
        fishItem = item;
        fishData = fishItem.fishData;
        icon.sprite = fishData.icon;
        priceText.text = fishData.sellPrice.ToString();
        selected = false;
        selectedMark.SetActive(false);
    }

    public void ToggleSelect()
    {
        selected = !selected;
        selectedMark.SetActive(selected);
    }

}
