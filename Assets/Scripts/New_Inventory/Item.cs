using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // Name of the item
    public Sprite itemIcon; // Icon to display in the inventory
    public GameObject prefabReference;

    private void Awake()
    {
        // If prefabReference is not already set, set it to this instance's prefab
        if (prefabReference == null)
        {
            prefabReference = gameObject; // Assign the prefab reference
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"Player entered trigger zone for item: {itemName}");
            InventoryManager.Instance.SetItemInRange(this); // Notify InventoryManager
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"Player exited trigger zone for item: {itemName}");
            InventoryManager.Instance.ClearItemInRange(); // Notify InventoryManager
        }
    }
}
