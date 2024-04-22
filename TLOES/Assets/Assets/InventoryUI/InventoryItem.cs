using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color chosenColor, notChosenColor;

   // public void Awake()
   // {
   //     Deselect();
   // }

    public void Select()
    {
        image.color = chosenColor;
    }
   public void Deselect()
   {
       image.color = notChosenColor;
   }


    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject Drop = eventData.pointerDrag;
            DragItem dragItem = Drop.GetComponent<DragItem>();
            dragItem.parentitemDrag = transform;
        }
    }

}
