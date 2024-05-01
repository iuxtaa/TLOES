using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItem : MonoBehaviour, IDropHandler
{

    public Image image;
    public Color pickedColor, NotPickedColor;

   
    public void pick()
    {
        image.color = pickedColor;

    }

    public void NotPick()
    {
        image.color = NotPickedColor;
    }



    public void UpdateUI()
    {

    }





    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            GameObject DropItem = eventData.pointerDrag;
            ItemInside itemInside = DropItem.GetComponent<ItemInside>();
            itemInside.AfterDrag = transform;
        }
    }
}
