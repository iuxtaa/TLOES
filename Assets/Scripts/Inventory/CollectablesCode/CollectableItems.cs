using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
  
    public Player player;
    private bool playerClose = false;
    public CollectableItemsType type;
    public Sprite icon;
    public Inventory inventoryCountCheck;
   
    public int changingnum = 0;
    public static int moneyAmount = 10;
    private const int HAM_COST = 6;
    private const int WINE_COST = 8;
    private const int APPLE_CONST = 2;
    private const int PAPER_CONST = 2;
    private const int QUILL_CONST = 3;

    //cost
    private const int HAM_SELL = 5;
    private const int WINE_SELL = 6;
    private const int APPLE_SELL = 1;

    public void Update()
    {
        BuyItem();
        SellItem();
        ReceiveItem();
        changingnum = moneyAmount;
    }

    public void ReceiveItem()
    {
        if(playerClose) // && player.GetQuest().isActive
        {
            if(this.gameObject.tag == "EGG")
            {
                player.inventory.Adding(this);
            }

            else if(this.gameObject.tag == "EMPTYBOTTLE")
            {
                player.inventory.Adding(this);
            }
        }
    }
   
    public void BuyItem()
    {
      if(playerClose && InputsHandler.GetInstance().buyButtonPressed())
        {
            if(this.gameObject.tag == "HAM")
            { 
                if(moneyAmount>= HAM_COST)
                {
                    player.inventory.Adding(this);
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
                    player.inventory.Adding(this);
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
                    player.inventory.Adding(this);
                    moneyAmount -= APPLE_CONST;
                    Debug.Log("Apple bought");
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
                
            }

            else if(this.gameObject.tag == "Paper")
            {
                if (moneyAmount>= PAPER_CONST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= PAPER_CONST;
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
            }

            else if(this.gameObject.tag == "Quill")
            {
                if (moneyAmount>= QUILL_CONST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= QUILL_CONST;
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
                    player.inventory.Removing(this);
                    moneyAmount += HAM_SELL;
                    Debug.Log("Ham Sold");
                }
            }

            else if (this.gameObject.tag == "Wine")
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.inventory.Removing(this);
                    moneyAmount += WINE_SELL;
                    Debug.Log("Wine Sold");
                }
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.inventory.Removing(this);
                    moneyAmount += APPLE_SELL;
                    Debug.Log("Apple Sold");
                }
                       
            }
        }

    }

    private bool CanRemoveItemFromInventory(CollectableItemsType itemType)
    {
        foreach (var slot in player.inventory.slots)
        {
            if (slot.type == itemType)
            {
                return player.inventory.CanRemoveItem(slot);
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
    NONE, HAM, APPLE, WINE, EGG, PAPER, QUILL, EMPTYBOTTLE, WATERBOTTLE
}
