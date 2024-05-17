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
   // public TempInventory inventoryCountCheck;
   
    public int changingnum = 0;
    public static int moneyAmount = 10;
    private int hamCost = 6;
    private int wineCost = 8;
    private int appleCost = 2;

    private int hamSell = 5;
    private int wineSell = 6;
    private int appleSell = 1;

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
                if(moneyAmount>= hamCost)
                {
                    player.tempInventory.Adding(this);
                    moneyAmount -= hamCost;
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
                    moneyAmount -= wineCost;
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
                    moneyAmount -= appleCost;
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
                    moneyAmount += hamSell;
                    Debug.Log("Ham Sold");
                }
            }

            else if (this.gameObject.tag == "Wine")
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.tempInventory.Removing(this);
                    moneyAmount += wineSell;
                    Debug.Log("Wine Sold");
                }
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.tempInventory.Removing(this);
                    moneyAmount += appleSell;
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
