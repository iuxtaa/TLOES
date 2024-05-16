using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hint : MonoBehaviour
{
    private int counter;
    private int waitTime = 26;
    public GameObject hint;
    private bool displayNewHint = false; 


    void Update()
    {
        if(!displayNewHint)
        {
            hint.GetComponent<Animator>().Play("New State");
            displayNewHint = true;
            StartCoroutine(hintTrack());
        }
        
    }

    IEnumerator hintTrack()
    {
        
        counter = Random.Range(1,11);
        if (counter == 1)
        {
            hint.GetComponent<Text>().text = "Use A & D to walk left and right";
        }
        if (counter == 2)
        {
            hint.GetComponent<Text>().text = "Use the SPACE bar to jump";
        }
        if (counter == 3)
        {
            hint.GetComponent<Text>().text = "While holding the SHIFT key, double click A or D to dash";
        }
        if (counter == 4)
        {
            hint.GetComponent<Text>().text = "Press T when next to an interactable NPC to talk to them";
        }
        if (counter == 5)
        {
            hint.GetComponent<Text>().text = "Use the pause menu to save the game before exiting";
        }
        if (counter == 6)
        {
            hint.GetComponent<Text>().text = "You can buy items from the vendors in the market in exchange for coins";
        }
        if (counter == 7)
        {
            hint.GetComponent<Text>().text = "Keep an eye out for dropped items, you could sell them for extra coins";
        }
        if (counter == 8)
        {
            hint.GetComponent<Text>().text = "Keep an eye out for easter eggs";
        }
        if (counter == 9)
        {
            hint.GetComponent<Text>().text = "Did you know, if you stacked elephants on each other, they wouldnt like it?";
        }
        if (counter == 10)
        {
            hint.GetComponent<Text>().text = "Our programmers are super tired, its currently 4am on a Tuesday.";
        }

        hint.GetComponent<Animator>().Play("Hint Animation");
        yield return new WaitForSeconds(waitTime);
        displayNewHint = false;  
    }
}
