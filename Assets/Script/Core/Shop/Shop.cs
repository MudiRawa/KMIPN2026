using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    public GameObject textE, dialog;
    private bool playerInTrigger = false, isShopOpen = false;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
    }

    void Update()
    {
        if (playerInTrigger && playerInput != null)
        {
            if (playerInput.actions["Interact"].WasPressedThisFrame())
            {
                Interact();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isShopOpen)
        {
            playerInTrigger = true;
            textE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInTrigger = false;
            textE.SetActive(false);
        }
    }

    private void Interact()
    {
        if (!isShopOpen)
        {
            textE.gameObject.SetActive(false);
            isShopOpen = true;
            Debug.Log("Shop opened!");
            dialog.SetActive(true);
            PlayerMovement.instance.DisableMovement();
        }
    }

    public void sellFish()
    {
        Inventory inventory = FindAnyObjectByType<Inventory>();
        InventoryUI inventoryUI = FindAnyObjectByType<InventoryUI>();
        CoinUI coinUI = FindAnyObjectByType<CoinUI>();

        if (inventory != null && inventory.fishCount > 0)
        {
            int fishToSell = inventory.fishCount;

            int earnedCoins = fishToSell * 100;

            // Tambahkan coin, bukan replace
            coinUI.AddCoins(earnedCoins);

            inventory.fishCount = 0;

            inventoryUI.UpdateInventoryUI(inventory.fishCount);

            Debug.Log("Sold " + fishToSell + " fish!");

            PlayerPrefs.SetInt("FishCount", inventory.fishCount);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("No fish to sell!");
        }
    }

    public void CloseShop()
    {
        isShopOpen = false;
    }
}
