using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    public Player player;
    public Vector2 playerResetPosition;
    public GameObject warningText;
    private const float INVOKE_OFFSET = 3f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !collision.isTrigger)
        {
            player.transform.position = playerResetPosition;

            if(!warningText.activeInHierarchy)
            {
                warningText.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Invoke("HideWarningText", INVOKE_OFFSET);
    }

    private void HideWarningText()
    {
        if(warningText.activeInHierarchy)
        {
            warningText.SetActive(false);
        }
    }
}

public enum MapZoneType {
    CHURCH, FARM, MARKET, WELL
}
