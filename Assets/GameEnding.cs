using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameEnding : MonoBehaviour
{
    public GameObject endingPanel;  // Reference to the panel that contains the text
    public TMP_Text endingText;     // Reference to the TextMeshPro component

    void Start()
    {
        // Initially hide the ending panel
        endingPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            EvaluateEnding(collider.gameObject); // Pass the Player GameObject
        }
    }

    public void DisplayEnding(string message)
    {
        endingText.text = message;   // Set the ending message
        endingPanel.SetActive(true); // Show the panel
    }

    private void EvaluateEnding(GameObject playerGameObject)
    {
        // Assuming Player.favourability is a static property
        if (Player.favourability > 5)
        {
            DisplayEnding("Congratulations! You've won with honor!");
        }
        else if (Player.favourability <= 0)
        {
            DisplayEnding("Unfortunately, your decisions have led to despair.");
        }
        else
        {
            DisplayEnding("The world remains unchanged by your actions.");
        }
    }
}
