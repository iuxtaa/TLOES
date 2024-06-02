using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotManager : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI amountText;
    

    public void SetItem(Inventory.Slot slot)
    {
        if(slot != null)
        {
            itemIcon.sprite = slot.Icon;
            itemIcon.color = new Color(1,1,1,1);
            amountText.text = slot.count.ToString();
        }
    }

    public void SetEmpty()//resets slot
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1,1,1,0);
        amountText.text = "";
    }
}
