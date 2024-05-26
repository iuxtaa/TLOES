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
    public CollectableItemsType type;
    public Sprite icon;
    public Inventory inventoryCountCheck;
    public GameObject popupText;


    public int changingnum = 0;
    public static int moneyAmount = 10;
    private const int HAM_COST = 6;
    private const int WINE_COST = 8;
    private const int APPLE_COST = 2;
    private const int PAPER_COST = 2;
    private const int QUILL_COST = 3;

    private const int HAM_SELL = 5;
    private const int WINE_SELL = 6;
    private const int APPLE_SELL = 1;
    private const int EGG_SELL = 2;

    public void Update()
    {
        BuyItem();
        SellItem();
        changingnum = moneyAmount;
    }
    public void BuyItem()
    {
        if (playerClose && InputsHandler.GetInstance().buyButtonPressed())
        {
            if (this.gameObject.tag.Equals(CollectableItemsType.HAM.ToString()))
            {
                if (moneyAmount >= HAM_COST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= HAM_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Ham has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy ham.";
                }
            }

            else if (this.gameObject.tag.Equals(CollectableItemsType.WINE.ToString()))
            {
                if (moneyAmount >= WINE_COST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= WINE_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Wine has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy wine.";
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.APPLE.ToString()))
            {
                if (moneyAmount >= APPLE_COST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= APPLE_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Apple has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy apple.";
                }

            }

            else if (this.gameObject.tag == "Paper")
            {
                if (moneyAmount>= PAPER_COST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= PAPER_COST;
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
            }

            else if (this.gameObject.tag == "Quill")
            {
                if (moneyAmount >= QUILL_COST)
                {
                    player.inventory.Adding(this);
                    moneyAmount -= QUILL_COST;
                }
                else
                {
                    Debug.Log("Not enought money message will be triggered here");
                }
            }
            popupText.SetActive(true);
        }
    }

    public void SellItem()
    {
        if (playerClose && InputsHandler.GetInstance().sellButtonPressed())
        {
            Debug.Log("V is pressed");
            if (this.gameObject.tag.Equals(CollectableItemsType.HAM.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.HAM))
                {
                    player.inventory.Removing(this);
                    moneyAmount += HAM_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a ham to Butch!";
                }
            }

            else if (this.gameObject.tag.Equals(CollectableItemsType.WINE.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.inventory.Removing(this);
                    moneyAmount += WINE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a wine to Jack!";
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.APPLE.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.inventory.Removing(this);
                    moneyAmount += APPLE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a apple to Kate!";
            }
            popupText.SetActive(true);
            }
        }
    }
    
    public bool SellQuestItem()
    {
        if (playerClose && InputsHandler.GetInstance().sellButtonPressed())
        {
            if(this.gameObject.tag.Equals(CollectableItemsType.EGG.ToString()))
            {
                if(CanRemoveItemFromInventory(CollectableItemsType.EGG))
                {
                    player.inventory.Removing(this);
                    moneyAmount += EGG_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold an egg!";
                }
            }
            popupText.SetActive(true);
            return true;
        }
        return false;
    }
    
    public void HidePopupText()
    {
        if(popupText.activeInHierarchy)
        {
            popupText.SetActive(false);       
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
