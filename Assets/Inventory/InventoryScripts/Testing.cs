using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    public InventoryController Controller;
    public CollectableItems[] Pickup;
    public GameObject pickupButtonObject; 

    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = pickupButtonObject.GetComponentInChildren<Image>(true);
        buttonText = pickupButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void PickupItem(int index)
    {
        // Check for valid index and null or default values to avoid errors
        if (index >= 0 && index < Pickup.Length && Pickup[index] != null && Controller != null)
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
                    Debug.Log("Cannot pick");
                }
            }
            else
            {
                Debug.Log("Cannot pick item, stack limit reached.");
                DisableButton();
            }
        }
        else
        {
            Debug.Log("Invalid item, cant pick up");
        }
    }

    public void DisableButton()
    {
        if (buttonImage != null)
        {
            var color = buttonImage.color;
            color.a = 0.5f;
            buttonImage.color = color;
        }

        if (buttonText != null)
        {
            buttonText.enabled = false;
        }

        if (pickupButtonObject != null)
        {
            pickupButtonObject.SetActive(false);
        }
    }
}
