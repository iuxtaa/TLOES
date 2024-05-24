using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
  
    public Player player;
    private bool playerClose = false;
    private bool showPopup = false;
    public CollectableItemsType type;
    public Sprite icon;
    public Inventory inventoryCountCheck;
    public GameObject popupText;

   
    public int changingnum = 0;
    public static int moneyAmount = 10;
    private const int HAM_COST = 6;
    private const int WINE_COST = 8;
    private const int APPLE_CONST = 2;
    private const int PAPER_CONST = 2;
    private const int QUILL_CONST = 3;

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
        if(playerClose)
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
                    popupText.GetComponent<TextMeshProUGUI>().text = "Ham has been added to your inventory.";
                    Debug.Log("Ham picked up");
                    popupText.SetActive(true);
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy ham.";
                    Debug.Log("Not enought money message will be triggered here");
                    popupText.SetActive(true);
                }
            }

            else if (this.gameObject.tag == "Wine")
            {
                if(moneyAmount>= WINE_COST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= WINE_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Wine has been added to your inventory.";
                    Debug.Log("Wine picked up");
                    popupText.SetActive(true);
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy wine.";
                    Debug.Log("Not enought money message will be triggered here");
                    popupText.SetActive(true);
                }  
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(moneyAmount>= APPLE_CONST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= APPLE_CONST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Apple has been added to your inventory.";
                    Debug.Log("Apple bought");
                    popupText.SetActive(true);
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy apple.";
                    Debug.Log("Not enought money message will be triggered here");
                    popupText.SetActive(true);
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
        // Invoke("HidePopupText", 5f);
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
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a ham to Butch!";
                    Debug.Log("Ham Sold");
                    popupText.SetActive(true);
                }
            }

            else if (this.gameObject.tag == "Wine")
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.inventory.Removing(this);
                    moneyAmount += WINE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a wine to Jack!";
                    Debug.Log("Wine Sold");
                    popupText.SetActive(true);
                }
            }
            else if (this.gameObject.tag == "Apple")
            {
                if(CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.inventory.Removing(this);
                    moneyAmount += APPLE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a apple to Kate!";
                    Debug.Log("Apple Sold");
                    popupText.SetActive(true);
                }
                       
            }
        }
        // Invoke("HidePopupText", 5f);
    }

    public void HidePopupText()
    {
        popupText.SetActive(false);
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
            Invoke("HidePopupText", 5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerClose = false;
            Invoke("HidePopupText", 5f);
        }
    }
}

public enum CollectableItemsType
{
    NONE, HAM, APPLE, WINE, EGG, PAPER, QUILL, EMPTYBOTTLE, WATERBOTTLE
}
