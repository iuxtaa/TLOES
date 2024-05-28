using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorialText;
    private bool playerClose = false;
    private const float INVOKE_OFFSET = 7.0f;

    private void HideTutorialText()
    {
        if(tutorialText.activeInHierarchy)
        {
            tutorialText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerClose = true;
            tutorialText.SetActive(playerClose);
            Invoke("HideTutorialText", INVOKE_OFFSET);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerClose = false;
            Invoke("HideTutorialText", INVOKE_OFFSET);
        }
    }
}
