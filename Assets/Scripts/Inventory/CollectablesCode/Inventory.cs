using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    [System.Serializable]
    public class Slot
    {
        // CONSTANT VARIABLES
        public const int MAX_DEFAULT_STACK = 5;
        public CollectableItemsType type;
        public int count;
        public Sprite Icon;

        public Slot()
        {
            type = CollectableItemsType.NONE;
            count = 0;
        }

        public bool CanAddItem()
        {
            if(count < MAX_DEFAULT_STACK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      

        public void AddItem(CollectableItems item)
        {
            this.type = item.type;
            this.Icon = item.icon;
            count++;
        }

        public void RemoveItem()
        {
            if(count >0)
            {
                count--;
                if(count ==0)
                {
                    Icon = null;
                    type = CollectableItemsType.NONE;
                }
            }
        }



    }
    public List<Slot> slots = new List<Slot>();
    public Inventory(int numSlots)
    {
        for(int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    public void Adding(CollectableItems item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.type == item.type && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach(Slot slot in slots)
        {
            if(slot.type == CollectableItemsType.NONE)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Removing(CollectableItems item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.type == item.type && CanRemoveItem(slot))
            {
                slot.RemoveItem();
                return;
            }
        }
    }
    public bool CanRemoveItem(Slot slot)
    {
        if (slot.count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }
}
