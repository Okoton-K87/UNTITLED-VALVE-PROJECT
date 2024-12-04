using UnityEngine;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI craftingMessage; // Message to display crafting options
    public GameObject craftedGun; // The gun prefab to enable after crafting

    private bool isPlayerInRange = false; // To check if the player is in range

    private void Start()
    {
        // Hide the crafting message initially
        if (craftingMessage != null)
        {
            craftingMessage.gameObject.SetActive(false);
        }

        // Ensure the craftedGun is inactive initially
        if (craftedGun != null)
        {
            craftedGun.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            // Check if inventory contains required items
            bool hasBody = InventoryManager.Instance.InventoryContains("M1A1_body");
            bool hasParts = InventoryManager.Instance.InventoryContains("M1A1_parts");

            if (hasBody && hasParts)
            {
                // Show crafting option
                craftingMessage.text = "Press C to craft M1A1.";
                craftingMessage.gameObject.SetActive(true);

                // Check if player presses the crafting button
                if (Input.GetKeyDown(KeyCode.C))
                {
                    // Craft the gun
                    CraftItem();
                }
            }
            else
            {
                // Show missing item message
                craftingMessage.text = $"Missing: {(hasBody ? "" : "M1A1_body")} {(hasParts ? "" : "M1A1_parts")}";
                craftingMessage.gameObject.SetActive(true);
            }
        }
        else
        {
            // Hide the message when out of range
            if (craftingMessage != null)
            {
                craftingMessage.gameObject.SetActive(false);
            }
        }
    }

    private void CraftItem()
    {
        Debug.Log("Crafting M1A1...");
        InventoryManager.Instance.RemoveItemByName("M1A1_body");
        InventoryManager.Instance.RemoveItemByName("M1A1_parts");

        // Enable the crafted gun
        if (craftedGun != null)
        {
            craftedGun.SetActive(true);
            Debug.Log("M1A1 crafted and placed on the workbench!");
        }

        // Update the crafting message
        if (craftingMessage != null)
        {
            craftingMessage.text = "M1A1 crafted!";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered crafting range.");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited crafting range.");
            isPlayerInRange = false;

            // Hide crafting message
            if (craftingMessage != null)
            {
                craftingMessage.gameObject.SetActive(false);
            }
        }
    }
}
