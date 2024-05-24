using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopup : MonoBehaviour
{
    public GameObject popupText;
    // public CollectableItems item;

    public void Update()
    {
        popupText.GetComponent<Animator>().Play("Default");
    }

    public void PopupMessages()
    {
        string message = "";

       
    }
    
}
