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
    public Inventory inventory;
    public GameObject popupText;


    public int changingnum = 0;
    private const int HAM_COST = 6;
    private const int WINE_COST = 8;
    private const int APPLE_COST = 2;
    private const int PAPER_COST = 3;

    private const int HAM_SELL = 5;
    private const int WINE_SELL = 6;
    private const int APPLE_SELL = 1;
    private const int EGG_SELL = 2;
    private const int PAPER_SELL = 2;

    private const float INVOKE_OFFSET = 3.5f;
    private bool canGiveToBeggar = true;
    private bool canSellToCecil = true;
    private bool canGiveToKnight = true;
    public static int amountGivenToBeggar = 1;
    public static int amountGivenToCecil = 4;
    public static int amountGivenToKnight = 1;
    public void Update()
    {
        BuyOrGetItem();
        SellOrGiveItem();
        changingnum = Player.money;
    }
    public void BuyOrGetItem()
    {
        if (playerClose && InputsHandler.GetInstance().buyButtonPressed())
        {
            if (this.gameObject.tag.Equals(CollectableItemsType.HAM.ToString()))
            {
                if (changingnum >= HAM_COST)
                {
                    player.inventory.Adding(this);
                    Player.money -= HAM_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Ham has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy ham.";
                }
            }

            else if (this.gameObject.tag.Equals(CollectableItemsType.WINE.ToString()))
            {
                if (changingnum >= WINE_COST)
                {
                    player.inventory.Adding(this);
                    Player.money -= WINE_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Wine has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy wine.";
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.APPLE.ToString()))
            {
                if (changingnum >= APPLE_COST)
                {
                    player.inventory.Adding(this);
                    Player.money -= APPLE_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Apple has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy apple.";
                }

            }

            else if (this.gameObject.tag.Equals(CollectableItemsType.PAPER.ToString()))
            {
                if (changingnum >= PAPER_COST)
                {
                    player.inventory.Adding(this);
                    Player.money -= PAPER_COST;
                    popupText.GetComponent<TextMeshProUGUI>().text = "Paper has been added to your inventory.";
                }
                else
                {
                    popupText.GetComponent<TextMeshProUGUI>().text = "You do not have enough money to buy paper.";
                }
            }
            else if(this.gameObject.tag.Equals(CollectableItemsType.WATERBOTTLE.ToString()))
            {
                if(this.gameObject.name == "WaterBottle_WishingWell")
                {
                    if(CanRemoveItemFromInventory(CollectableItemsType.EMPTYBOTTLE))
                    {
                        player.inventory.Removing(CollectableItemsType.EMPTYBOTTLE);
                        player.inventory.Adding(this);
                        popupText.GetComponent<TextMeshProUGUI>().text = "Holy water has been added to your inventory.";
                    }
                    else
                    {
                        popupText.GetComponent<TextMeshProUGUI>().text = "You do not have any empty bottles in your inventory.";
                    }
                }
            }
            popupText.SetActive(true);
        }
    }

    public void SellOrGiveItem()
    {
        if (playerClose && this.gameObject.name == "Egg_Begger" && canGiveToBeggar)
        {
            player.inventory.Removing(this);
            this.gameObject.SetActive(false);
            if (canGiveToBeggar)
            {
                popupText.GetComponent<TextMeshProUGUI>().text = "BEGGER says 'Thanks Bud' ";
                popupText.SetActive(true);
                DialogueScript.GetInstance().turnOffColliderBegger();
            }
            canGiveToBeggar = false;
            Debug.Log(canGiveToBeggar);
        }
        if (playerClose && InputsHandler.GetInstance().sellButtonPressed())
        {
            
            Debug.Log("V is pressed");
            if (this.gameObject.tag.Equals(CollectableItemsType.HAM.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.HAM))
                {
                    player.inventory.Removing(this);
                    Player.money += HAM_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a ham to Butch!";
                    popupText.SetActive(true);
                }
            }

            else if (this.gameObject.tag.Equals(CollectableItemsType.WINE.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.inventory.Removing(this);
                    Player.money += WINE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a wine to Jack!";
                    popupText.SetActive(true);
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.APPLE.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.inventory.Removing(this);
                    Player.money += APPLE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a apple to Kate!";
                    popupText.SetActive(true);
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.PAPER.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.PAPER))
                {
                    if(this.gameObject.name == "Paper_Knight" && canGiveToKnight)
                    {
                        player.inventory.Removing(this);
                        if(canGiveToKnight)
                        {
                            popupText.GetComponent<TextMeshProUGUI>().text = "You gave paper to the Knight!";
                            popupText.SetActive(true);
                        }
                        canGiveToKnight = false;
                    }
                    else 
                    {
                        player.inventory.Removing(this);
                        Player.money += PAPER_SELL;
                        popupText.GetComponent<TextMeshProUGUI>().text = "You sold a paper to Patrick!";
                        popupText.SetActive(true);
                    }
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.EGG.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.EGG))
                {
                    if (this.gameObject.name == "Egg_Cecil" && canSellToCecil)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            player.inventory.Removing(this);
                            Player.money += EGG_SELL;
                        }
                        if (canSellToCecil)
                        {
                            popupText.GetComponent<TextMeshProUGUI>().text = "You sold some eggs to Cecil!";
                            popupText.SetActive(true);
                            DialogueScript.GetInstance().turnOffColliderCecil();
                        }
                        canSellToCecil = false;
                    }
                   
                }
            }
        }
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
            Invoke("HidePopupText", INVOKE_OFFSET);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerClose = false;
            Invoke("HidePopupText", INVOKE_OFFSET);
        }
    }
}

public enum CollectableItemsType
{
    NONE, HAM, APPLE, WINE, EGG, PAPER, QUILL, EMPTYBOTTLE, WATERBOTTLE
}
