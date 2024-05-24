using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapBoundaryManager : MonoBehaviour
{
    public VectorValue playerPosition;
    public Player player;
    public GameObject warningText;
    private const float INVOKE_OFFSET = 5f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !collision.isTrigger)
        {
            player.transform.position = playerPosition.initialValue;

            if(!warningText.gameObject.activeInHierarchy)
            {
                warningText.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(warningText.gameObject.activeInHierarchy)
        {
            Invoke("HideWarningText", INVOKE_OFFSET);
        }
    }

    private void HideWarningText()
    {
        warningText.SetActive(false);
    }
}
