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
    private const int PAPER_COST = 3;

    private const int HAM_SELL = 5;
    private const int WINE_SELL = 6;
    private const int APPLE_SELL = 1;
    private const int EGG_SELL = 2;
    private const int PAPER_SELL = 2;

    private const float INVOKE_OFFSET = 3.5f;
    int index = 0;
    public void Update()
    {
        BuyItem();
        SellItem();
        SellQuestItem();
        changingnum = Player.money;
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
                if (moneyAmount >= WINE_COST)
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
                if (moneyAmount >= APPLE_COST)
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
                if (moneyAmount>= PAPER_COST)
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
                    Player.money += HAM_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a ham to Butch!";
                }
            }

            else if (this.gameObject.tag.Equals(CollectableItemsType.WINE.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.WINE))
                {
                    player.inventory.Removing(this);
                    Player.money += WINE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a wine to Jack!";
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.APPLE.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.APPLE))
                {
                    player.inventory.Removing(this);
                    Player.money += APPLE_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a apple to Kate!";
                }
            }
            else if (this.gameObject.tag.Equals(CollectableItemsType.PAPER.ToString()))
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.PAPER))
                {
                    player.inventory.Removing(this);
                    Player.money += PAPER_SELL;
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold a paper to Patrick!";
                }
            }
            else if(this.gameObject.tag.Equals(CollectableItemsType.EGG.ToString()))
            {
                Debug.Log("Level 2");
                
                if (CanRemoveItemFromInventory(CollectableItemsType.EGG) && index <1 )
                {
                    index++;
                    Debug.Log("Level 3");
                    for (int i = 0; i < 4; i++)
                    { 
                        player.inventory.Removing(this);
                        Player.money += EGG_SELL;
                        Debug.Log("Level 4");
                    }

                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold some eggs!";
                }
            }
            else if (this.gameObject.tag.Equals("BEGGEREGG"))//one egg is given to the begger
            {
                if (CanRemoveItemFromInventory(CollectableItemsType.EGG))
                {
                    for (int i = 0; i < 1; i++)
                    {
                        player.inventory.Removing(this);
                        gameObject.SetActive(false);
                    }
                    popupText.GetComponent<TextMeshProUGUI>().text = "BEGGER says 'Thanks Bud' ";
                    
                }
            }

            popupText.SetActive(true);
        }
    }
    
    public bool SellQuestItem()
    {
        if (playerClose && InputsHandler.GetInstance().sellButtonPressed())
        {
            Debug.Log("Level 1");
            if(this.gameObject.tag.Equals(CollectableItemsType.EGG.ToString()))
            {
                Debug.Log("Level 2");
                if (CanRemoveItemFromInventory(CollectableItemsType.EGG))
                {
                    Debug.Log("Level 3");
                    for (int i = 0; i < 4; i++)
                    {
                        player.inventory.Removing(this);
                        moneyAmount += EGG_SELL;
                        Debug.Log("Level 4");
                    }
                    popupText.GetComponent<TextMeshProUGUI>().text = "You sold some eggs!";
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
