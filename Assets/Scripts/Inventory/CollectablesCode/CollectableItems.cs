using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
  //this script connects to the item you want to buy
    public Player player;
    private bool playerClose =false;
    public CollectableItemsType type;
    public Sprite icon;
    public Inventory inventoryCountCheck;
   
    public int changingnum = 0;
    public static int moneyAmount = 10;
    private const int HAM_COST = 6;
    private const int WINE_COST = 8;
    private const int APPLE_CONST = 2;

    private const int HAM_SELL = 5;
    private const int WINE_SELL = 6;
    private const int APPLE_SELL = 1;

    public void Update()
    {
        BuyItem();
        SellItem();
        changingnum = moneyAmount;
    }

    
   
    public void BuyItem()
    {
      if(playerClose && InputsHandler.GetInstance().buyButtonPressed())
        {
            if(this.gameObject.tag == "HAM")
            { 
                if(moneyAmount>= HAM_COST)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= HAM_COST;
                    Debug.Log("Ham picked up");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }    
            }

            else if (this.gameObject.tag == "Wine")
            {
                if(moneyAmount>= WINE_COST)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= WINE_COST;
                    Debug.Log("Wine picked up");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }  
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(moneyAmount>= APPLE_CONST)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= APPLE_CONST;
                    Debug.Log("Apple bought");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
                
            }
        }
    }

   public void SellItem()
    {
        if (playerClose && InputsHandler.GetInstance().sellButtonPressed())
        {
            Debug.Log("V is pressed");
            if (this.gameObject.tag == "HAM")
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.HAM))
                {
                    player.tempInventory.Removing(this);
                    moneyAmount += HAM_SELL;
                    Debug.Log("Ham Sold");
                }
            }

            else if (this.gameObject.tag == "Wine")
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.tempInventory.Removing(this);
                    moneyAmount += WINE_SELL;
                    Debug.Log("Wine Sold");
                }
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.tempInventory.Removing(this);
                    moneyAmount += APPLE_SELL;
                    Debug.Log("Apple Sold");
                }
                       
            }
        }
    }

    private bool CanRemoveItemFromInventory(CollectableItemsType itemType)
    {
        foreach (var slot in player.tempInventory.slots)
        {
            if (slot.type == itemType)
            {
                return player.tempInventory.CanRemoveItem(slot);
            }
        }
        return false;
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
