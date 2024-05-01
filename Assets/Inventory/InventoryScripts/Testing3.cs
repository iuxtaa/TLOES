using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;


public class Testing3 : MonoBehaviour
{
    public Controller controller;  
    public GameObject discardButtonObject; 
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
