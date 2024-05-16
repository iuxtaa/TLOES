using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Controller : MonoBehaviour
{
    public static Controller Instance; // Singleton instance
    public GameObject inventoryUI; // Reference to the inventory UI GameObject
    public Transform itemsParent; // Parent object to hold UI item slots
    public GameObject itemSlotPrefab; // Prefab for item slot UI
    private List<InventoryItem> items = new List<InventoryItem>();
    private bool isInventoryOpen = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isInventoryOpen); // Ensure the inventory is closed at the start
        }
    }

    void Start()
    {
      //  if (inventoryUI != null)
      //  {
      //      inventoryUI.SetActive(isInventoryOpen); // Ensure the inventory is closed at the start
      //  }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory();
        }

        // Test: Add a dummy item when P key is pressed (for pickup simulation)
        if (Input.GetKeyDown(KeyCode.P))
        {
            Items testItem = ScriptableObject.CreateInstance<Items>();
            testItem.Type = Items.ItemType.Coin;
            testItem.Image = null; // Replace with actual sprite
            AddItem(testItem, 1); // Replace with actual item data and quantity
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isInventoryOpen);
        }
    }

    public void AddItem(Items itemData, int quantity)
    {
        GameObject itemObject = new GameObject(itemData.name);
        ItemInside itemInside = itemObject.AddComponent<ItemInside>();
        itemInside.InitialiseItem(itemData, quantity);

        InventoryItem newItem = itemObject.AddComponent<InventoryItem>();
        newItem.itemInside = itemInside;
        items.Add(newItem);
        itemObject.transform.SetParent(itemsParent); // Ensure the item is parented under the itemsParent
        UpdateInventoryUI();
    }

    public void RemoveItem(string itemName, int quantity)
    {
        var itemsList = new List<InventoryItem>(itemsParent.GetComponentsInChildren<InventoryItem>());
        int removedCount = 0;

        for (int i = 0; i < itemsList.Count; i++)
        {
            InventoryItem it = itemsList[i];
            if (it != null && it.itemInside.items.name == itemName)
            {
                items.Remove(it);
                Destroy(it.gameObject);
                removedCount++;
                if (removedCount >= quantity) break;
            }
        }

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // Clear current UI slots
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Create new UI slots
        foreach (InventoryItem item in items)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, itemsParent);
            InventoryItem slotScript = newSlot.GetComponent<InventoryItem>();
            if (slotScript != null)
            {
                slotScript.itemInside = item.itemInside;
                slotScript.UpdateUI();
            }
        }
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.itemInside.items.name == itemName)
            {
                count += item.itemInside.Count;
            }
        }
        return count;
    }

    public void DiscardItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            InventoryItem itemToDiscard = items[index];
            items.RemoveAt(index);
            Destroy(itemToDiscard.gameObject);
            UpdateInventoryUI();
        }
    }
}
