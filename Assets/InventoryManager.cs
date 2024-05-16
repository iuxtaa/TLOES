using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class InventoryManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Controller.Instance.ToggleInventory();
        }

        // Test: Add a dummy item when P key is pressed (for pickup simulation)
        if (Input.GetKeyDown(KeyCode.P))
        {
            Items testItem = ScriptableObject.CreateInstance<Items>();
            testItem.Type = Items.ItemType.Coin;
            testItem.Image = null; // Replace with actual sprite
            Controller.Instance.AddItem(testItem, 1); // Replace with actual item data and quantity
        }
    }
}
