using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInventoryUI : MonoBehaviour
{
    public GameObject tempInventoryPanel;
    public Player player;
    public List<SlotManager> slots = new List<SlotManager>();
   
    // Update is called once per frame
    void Update()
    {
        if(InputsHandler.GetInstance().inventoryButtonPressed())
        {
            ToggleTempInventory();
        }
        Refresh();
    }

    public void ToggleTempInventory()
    {
        
        if (!tempInventoryPanel.activeSelf)
        {
            tempInventoryPanel.SetActive(true);
           
        }
        else
        {
            tempInventoryPanel.SetActive(false);   
        }
    }

    public void Refresh()
    {
        if(slots.Count == player.inventory.slots.Count)
        {
            for(int i= 0; i<slots.Count; i++)
            {
                if (player.inventory.slots[i].type != CollectableItemsType.NONE)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove(int slotNUM)
    {  
            player.inventory.Remove(slotNUM);
    }

}
