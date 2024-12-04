using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public TMP_Text itemNameText; // Text element for the item's name
    public Image itemIcon; // Image element for the item's icon

    // Method to initialize the button with item data
    public void Initialize(string itemName, Sprite itemIconSprite)
    {
        // Set the item's name on the button
        itemNameText.text = itemName;
        Debug.Log($"Setting button name to: {itemName}");

        if (itemIconSprite != null)
        {
            // Set the item's icon
            itemIcon.sprite = itemIconSprite;
            itemIcon.color = Color.white; // Ensure the color is visible
            itemIcon.gameObject.SetActive(true); // Activate the icon
            Debug.Log($"Setting button icon for: {itemName}, Icon: {itemIconSprite.name}");
        }
        else
        {
            // If no icon is provided, hide the icon
            itemIcon.gameObject.SetActive(false);
            Debug.LogWarning($"No icon assigned for item: {itemName}");
        }

        // Debugging logs
        Debug.Log($"Icon GameObject Active: {itemIcon.gameObject.activeSelf}");
    }


}
