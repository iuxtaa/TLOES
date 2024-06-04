using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // Include this for TextMeshPro




public class WellTrigger : MonoBehaviour
{
    public Player player; // Reference to the Player component
    public GameObject wellPopupText; // GameObject that contains the text component
    // public GameObject endingPanel; // Reference to the panel that shows ending text
    // public TMP_Text endingText; // Reference to the TextMeshPro component on the panel

    // private void Start()
    // {
    //     endingPanel.SetActive(false); // Initially hide the ending panel
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            wellPopupText.SetActive(true); // Show the well popup
            // UpdateWellText();
            // StartCoroutine(ShowEndingAfterDelay(5)); // Wait for 5 seconds before showing the ending
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

    private void UpdateWellText()
    {
        bool canUpdateWell = false;
        foreach(Quest quest in player.questHistory)
        {
            if(quest.completionStatus)
            {
                canUpdateWell = true;
            }
        }

        if(canUpdateWell)
        {
            wellPopupText.GetComponent<TextMeshProUGUI>().text = "Press 'Q' to make a wish in the well for 1 coin";
        }
    }

    // IEnumerator ShowEndingAfterDelay(float delayInSeconds)
    // {
    //     yield return new WaitForSeconds(delayInSeconds); // Wait for the specified delay

    //     // Evaluate the ending based on the player's favourability
    //     string message;
    //     if (Player.favourability > 5)
    //     {
    //         message = "Congratulations You've won with honor!";
    //     }
    //     else if (Player.favourability <= 0)
    //     {
    //         message = "Unfortunately, your decisions have led to despair.";
    //     }
    //     else
    //     {
    //         message = "The world remains unchanged by your actions.";
    //     }

    //     // Set the ending message and display it
    //     endingText.text = message;
    //     endingPanel.SetActive(true);

        
    // }
}
