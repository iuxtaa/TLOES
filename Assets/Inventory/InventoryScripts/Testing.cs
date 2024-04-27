using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    public Controller Controller;
    public Items[] Pickup;
    public GameObject pickupButtonObject; 

    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = pickupButtonObject.GetComponentInChildren<Image>(true);
        buttonText = pickupButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void Pickupitem(int index)
    {
        if (Controller.CanAddItem(Pickup[index]))
        {
            bool result = Controller.AddItem(Pickup[index]);
            if (result)
            {
                Debug.Log("Picked Item");
            }
            else
            {
                Debug.Log("Can Not pick");
            }
        }
        else
        {
            Debug.Log("Cannot pick item, stack limit reached.");
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

       
        pickupButtonObject.SetActive(false);
    }
}
