using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public TMP_Text itemNameText; // Text element for the item's name
    public Image itemIcon; // Image element for the item's icon

    // Field to store the item's name
    public string itemName { get; private set; }

    // Method to initialize the button with item data
    public void Initialize(string itemName, Sprite itemIconSprite)
    {
        // Set the item's name on the button
        this.itemName = itemName; // Store the item name
        itemNameText.text = itemName;

        if (itemIconSprite != null)
        {
            // Set the item's icon
            itemIcon.sprite = itemIconSprite;
            itemIcon.color = Color.white; // Ensure the color is visible
            itemIcon.gameObject.SetActive(true); // Activate the icon
        }
        else
        {
            // If no icon is provided, hide the icon
            itemIcon.gameObject.SetActive(false);
        }
    }
}
