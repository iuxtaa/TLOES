using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public Player player;
    public Vector2 newPlayerPosition;
    // public Vector2 exitPlayerPosition;
    public PlayerVectorValue playerPosition;
    public GameObject teleportationPromptMessage;
    private bool playerClose = false;

    public void Update()
    {
        TeleportPlayer();
    }

    public void TeleportPlayer()
    {
        if(playerClose && InputsHandler.GetInstance().teleportButtonPressed())
        {
            if(this.gameObject.tag.Equals(MapZoneType.FARM.ToString()))
            {
                playerPosition.changingValue = newPlayerPosition;
            }
            else if(this.gameObject.tag.Equals(MapZoneType.MARKET.ToString()))
            {
                playerPosition.changingValue = newPlayerPosition;
            }
            else if(this.gameObject.tag.Equals(MapZoneType.CHURCH.ToString()))
            {
                playerPosition.changingValue = newPlayerPosition;
            }
            else if(this.gameObject.tag.Equals(MapZoneType.WELL.ToString()))
            {
                playerPosition.changingValue = newPlayerPosition;
            }

            player.transform.position = playerPosition.changingValue;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !collision.isTrigger)
        {
            playerClose = true;
            teleportationPromptMessage.SetActive(playerClose);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerClose = false;
            teleportationPromptMessage.SetActive(playerClose);
        }
    }
}
