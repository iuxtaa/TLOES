using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;


public class Testing3 : MonoBehaviour
{
    public Controller controller;  // Make sure this is correctly assigned in the Unity Inspector
    public GameObject discardButtonObject; // Assign this in the Inspector
    public Items[] discard;
    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = discardButtonObject.GetComponentInChildren<Image>(true);
        buttonText = discardButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
        
        UpdateButtonState();
    }

    
    public void DiscardOneItem()
    {

        if (controller.Item.Length > 0)
        {
            controller.DiscardItem(0);
            Debug.Log("One item discarded");
            UpdateButtonState();
        }
        else
        {
            Debug.Log("No items to discard");
        }
    }

   
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
