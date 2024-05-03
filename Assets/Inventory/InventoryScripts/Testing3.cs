using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;



public class Testing3 : MonoBehaviour
{
    public Controller controller;
    public GameObject discardButtonObject; // Assign this in the Inspector

    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = discardButtonObject.GetComponentInChildren<Image>(true);
        buttonText = discardButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
        UpdateButtonState(); // Check if button should be active on game start
    }

    // Call this method when the Discard button is pressed
    public void DiscardOneItem()
    {

        int itemToDiscardIndex = -1;
        for (int i = 0; i < controller.Item.Length; i++)
        {
            if (controller.Item[i] != null && controller.Item[i].GetComponentInChildren<ItemInside>() != null)
            {
                itemToDiscardIndex = i;
                break;
            }
        }

        if (itemToDiscardIndex != -1)
        {
            controller.DiscardItem(itemToDiscardIndex);
            UpdateButtonState();
        }
        else
        {
            Debug.Log("No items to discard");
            discardButtonObject.SetActive(false);
        }
    }

    // Method to update the active state and appearance of the discard button
    private void UpdateButtonState()
    {
        if (controller.Item.Length > 0)
        {
            discardButtonObject.SetActive(true);
        }
        else
        {
            discardButtonObject.SetActive(false);
        }
    }
}

