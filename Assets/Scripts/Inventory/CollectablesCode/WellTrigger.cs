using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // Include this for TextMeshPro




public class WellTrigger : MonoBehaviour
{
    public Player player; // Reference to the Player component
    public GameObject wellPopupText; // GameObject that contains the text component


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            wellPopupText.SetActive(true); // Show the well popup
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            wellPopupText.SetActive(false); // Hide the well popup
            // No longer deactivate the ending panel here
        }
    }
}
