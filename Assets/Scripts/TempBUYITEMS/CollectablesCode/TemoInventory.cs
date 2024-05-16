using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TempInventory 
{
    [System.Serializable]
    public class Slot
    {
        public CollectableItemsType type;
        public int count;
        public int MaxAllowed;
        public Sprite Icon;

        public Slot()
        {
            type = CollectableItemsType.NONE;
            count = 0;
            MaxAllowed = 3;
        }

        public bool CanAddItem()
        {
            if(count < MaxAllowed)
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
    public TempInventory(int numSlots)
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

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }
}
