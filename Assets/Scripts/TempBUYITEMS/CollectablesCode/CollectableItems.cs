using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
  
    public Player player;
    private bool playerClose =false;
    public CollectableItemsType type;
    public Sprite icon;
   
    public int changingnum = 0;
    public static int moneyAmount = 10;
    private int hamCost = 5;
    private int wineCost = 7;
    private int appleCost = 1;

    public void Update()
    {
        PickupItem(); 
        changingnum = moneyAmount;
    }

    
   
    public void PickupItem()
    {
      if(playerClose && InputsHandler.GetInstance().buyButtonPressed())
        {
            if(this.gameObject.tag == "HAM")
            { 
                if(moneyAmount>= hamCost)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= 5;
                    Debug.Log("Ham picked up");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
                
                
                
            }
            else if (this.gameObject.tag == "Wine")
            {
                if(moneyAmount>= wineCost)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= 6;
                    Debug.Log("Wine picked up");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
                
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(moneyAmount>= appleCost)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= 1;
                    Debug.Log("Apple bought");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
                
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerClose = false;
        }
    }
}

public enum CollectableItemsType
{
    NONE, HAM, APPLE, WINE
}
