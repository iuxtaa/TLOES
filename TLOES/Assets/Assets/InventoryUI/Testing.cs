using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Manager manager;
    public Items[] pickup;

    public void PickupItem(int index)
    {
        if (index >= 0 && index < pickup.Length)
        {
            manager.AddItem(pickup[index]);
            Debug.Log("Item picked up: " + pickup[index].type);
        }
        else
        {
            Debug.Log("Invalid index for pickup.");
        }
    }

    public void GetChosenItem()
    {
        DragItem chosenItem = manager.GetChosenItem(false);
        if (chosenItem != null)
        {
            Debug.Log("Chosen item: " + chosenItem.items.type);
        }
        else
        {
            Debug.Log("No item selected.");
        }
    }

    public void UseChosenItem()
    {
        DragItem usedItem = manager.UseChosenItem(true);
        if (usedItem != null)
        {
            Debug.Log("Item used: " + usedItem.items.type);
        }
        else
        {
            Debug.Log("No item selected to use.");
        }
    }
}






