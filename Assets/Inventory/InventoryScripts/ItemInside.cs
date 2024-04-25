
using System.Diagnostics;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;


public class ItemInside : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image Picture;
    public TextMeshProUGUI countText;

    [HideInInspector] public Items items;
    [HideInInspector] public Transform AfterDrag;
    public int count = 1;

    public int Count
    {
        get { return count; }
        set
        {
            count = value;
       //     Refreshcount();
        }
    }


    public void InitialiseItem(Items Newitems)
    {
        items = Newitems;
        Picture.sprite = Newitems.Image;
    
    }

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Picture.raycastTarget = false;
        AfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        transform.SetParent(AfterDrag);
        Picture.raycastTarget = true;
    }
}
