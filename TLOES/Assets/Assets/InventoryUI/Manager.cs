using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Build;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int Stackedmax = 7;
    public InventoryItem[] InventoryItems;
    public GameObject item1Prefab;

    private int chosenItemIndex = -1;
    private List<Items> itemsList;

    private void Start()
    {
        itemsList = new List<Items>();
        InventoryItems = GetComponentsInChildren<InventoryItem>();
        AddItem(ScriptableObject.CreateInstance<Items>());
        ChangeChosenItem(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int value);
            if (isNumber && value > 0 && value <= InventoryItems.Length)
            {
                ChangeChosenItem(value - 1);
            }
        }
    }

    private void ChangeChosenItem(int newIndex)
    {
        if (chosenItemIndex >= 0 && chosenItemIndex < InventoryItems.Length)
        {
            InventoryItems[chosenItemIndex].Deselect();
        }

        if (newIndex >= 0 && newIndex < InventoryItems.Length)
        {
            InventoryItems[newIndex].Select();
            chosenItemIndex = newIndex;
        }
    }

    public void AddItem(Items newItem)
    {
        bool stacked = StackItem(newItem);
        if (!stacked)
        {
            if (itemsList.Count < Stackedmax)
            {
                itemsList.Add(newItem);
            }
            else
            {
                Debug.Log("Inventory is full.");
            }
            
        }
    }

    private bool StackItem(Items newItem)
    {
        foreach (Items existingItem in itemsList)
        {
            if (existingItem.GetType() == newItem.GetType() && existingItem.count < Stackedmax)
            {
                existingItem.count++;
                return true;
            }
        }
        return false;
    }

    public DragItem GetChosenItem(bool useItem)
    {
        
        if (chosenItemIndex >= 0 && chosenItemIndex < InventoryItems.Length)
        {
            InventoryItem inventoryItem = InventoryItems[chosenItemIndex];
            DragItem dragItem = inventoryItem.GetComponent<DragItem>();
            if (dragItem != null)
            {
                Items item = dragItem.items;
                if (useItem)
                {
                    dragItem.Count--;
                    if (dragItem.Count <= 0)
                    {
                        itemsList.Remove(item);
                        Destroy(dragItem.gameObject);
                    }
                    else
                    {
                        dragItem.ResetCount();
                    }
                }
                return dragItem;
            }
        }
        return null;
    }

    internal DragItem UseChosenItem(bool useItem)
    {
        return GetChosenItem(useItem);
    }

    public void FindItemSpace(Items item, InventoryItem inventoryItem)
    {
        GameObject itemInstance = Instantiate(item1Prefab, inventoryItem.transform);
        DragItem dragItem = itemInstance.GetComponent<DragItem>();
        if (dragItem != null)
        {
            dragItem.InitializeItems(item, 1);
        }
    }
}




