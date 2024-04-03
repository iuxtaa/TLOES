using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject floatingIcon;

    private bool playerClose;

    private void Awake()
    {
        playerClose = false;
        floatingIcon.SetActive(false);
    }

    private void Update()
    {
        if (playerClose)
        {
            floatingIcon.SetActive(true);
        }
        else
        {
            floatingIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerClose=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerClose=false;
        }
    }
}
