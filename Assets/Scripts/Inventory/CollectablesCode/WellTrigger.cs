using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellTrigger : MonoBehaviour
{
    public Player player;
    public GameObject inventory;
    public GameObject wellPopupText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            wellPopupText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            wellPopupText.SetActive(false);
        }
    }
}
