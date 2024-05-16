using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;





public class InventoryItem : MonoBehaviour, IDropHandler
{
    public ItemInside itemInside;

    void Awake()
    {
        itemInside = GetComponentInChildren<ItemInside>();
    }

    public void UpdateUI()
    {
        if (itemInside != null)
        {
            itemInside.RefreshCount();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropItem = eventData.pointerDrag;
        if (dropItem != null)
        {
            ItemInside draggedItemInside = dropItem.GetComponent<ItemInside>();
            if (draggedItemInside != null && transform.childCount == 0)
            {
                draggedItemInside.AfterDrag = transform;
                dropItem.transform.SetParent(transform);
                dropItem.transform.position = transform.position;

                // Update the UI of the dropped item
                UpdateUI();
            }
        }
    }
}
