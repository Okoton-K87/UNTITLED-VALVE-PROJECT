using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public GameObject inventoryUI; // Reference to the UI panel that shows the inventory
    public TextMeshProUGUI inventoryText; // Text element to list inventory items
    private List<string> items = new List<string>();

    private void Awake()
    {
        // Set up Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Hide the inventory UI initially
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        UpdateInventoryUI();
    }

    public void ToggleInventory()
    {
        if (inventoryUI != null)
        {
            bool isActive = inventoryUI.activeSelf;
            inventoryUI.SetActive(!isActive);
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI()
    {
        // Update the text to show all items in the inventory
        if (inventoryText != null)
        {
            inventoryText.text = "Inventory:\n" + string.Join("\n", items);
        }
    }
}
