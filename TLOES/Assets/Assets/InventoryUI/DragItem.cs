using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Items items;
    public Image image;
    public TextMeshProUGUI countText;
    [HideInInspector] public Transform parentitemDrag;

    // Modify count to be private with a public property
    private int _count = 1;
    public int Count
    {
        get { return _count; }
        set
        {
            _count = value;
            ResetCount();
        }
    }

    // Initialize the DragItem with the item and count
    public void InitializeItems(Items newItem, int itemCount)
    {
        items = newItem;
        image.sprite = newItem.Image;
        Count = itemCount;
        ResetCount();
    }

    // Update count text and its visibility based on count value
    public void ResetCount()
    {
        countText.text = Count.ToString();
        bool textActive = Count > 1;
        Count = Random.Range(0, Count);
        countText.gameObject.SetActive(textActive);
    }

    // Implement interface methods for dragging behavior
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start Drag");
        parentitemDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("on Drag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");
        transform.SetParent(parentitemDrag);
        image.raycastTarget = true;
    }
}
