
using System.Diagnostics;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Debug = UnityEngine.Debug;







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
            RefreshCount();
        }
    }

    public void InitialiseItem(Items newItem, int quantity = 1)
    {
        if (newItem != null)
        {
            items = newItem;
            Picture.sprite = newItem.Image;
            Count = quantity;
        }
        else
        {
            Debug.LogWarning("Newitems is null!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Picture.raycastTarget = false;
        AfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(AfterDrag);
        Picture.raycastTarget = true;
    }

    public void RefreshCount()
    {
        if (countText != null)
        {
            countText.text = count.ToString();
        }
    }
}
