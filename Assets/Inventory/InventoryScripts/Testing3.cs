using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;



public class Testing3 : MonoBehaviour
{
    public Controller Controller;
    public GameObject DiscardButtonObject; // Assign this in the Inspector

    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = DiscardButtonObject.GetComponentInChildren<Image>(true);
        buttonText = DiscardButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
        UpdateButtonState(); // Check if button should be active on game start
    }

    // Call this method when the Discard button is pressed
    public void DiscardOneItem()
    {
        // Check if there are items to discard
        if (Controller.inventoryItems.Count > 0)
        {
            Controller.DiscardItem(0); // Discard the first item in the list
            Debug.Log("One item discarded");
            UpdateButtonState(); // Update button state after discarding
        }
        else
        {
            Debug.Log("No items to discard");
        }
    }

    // Method to update the active state and appearance of the discard button
    private void UpdateButtonState()
    {
        bool hasItems = Controller.inventoryItems.Count > 0;
        DiscardButtonObject.SetActive(hasItems);

        if (buttonImage)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, hasItems ? 1.0f : 0.5f);
        }

        if (buttonText)
        {
            buttonText.enabled = hasItems;
        }
    }
}
