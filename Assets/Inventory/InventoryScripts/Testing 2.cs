using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class Testing2 : MonoBehaviour
{
   

    public InventoryController Controller;
    public Items[] Store;
    public GameObject StoreButtonObject;

    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = StoreButtonObject.GetComponentInChildren<Image>(true);
        buttonText = StoreButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void StoreItem(Items itemToStore)
    {
        
        if (Controller.CanAddItem(itemToStore))
        {
            bool result = Controller.AddItem(itemToStore);
            if (result)
            {
                Debug.Log("Stored Item");
            }
            else
            {
                Debug.Log("Cannot store more item, stack limit reached.");
                DisableButton();
            }
        }
        else
        {
            Debug.Log("Cannot store item, stack limit reached.");
            DisableButton();
        }
    }


    public void DisableButton()
    {
        if (buttonImage)
        {
            var color = buttonImage.color;
            color.a = 0.5f;
            buttonImage.color = color;
        }

        if (buttonText)
        {
            buttonText.enabled = false;
        }


        StoreButtonObject.SetActive(false);
    }
}
