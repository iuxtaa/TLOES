using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu (menuName = "Item/Items")] 
public class Items : ScriptableObject
{
   
    public GameObject[] items;

    
    public ItemType Type;

    [Header("UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite Image;

    public enum ItemType
    {
        Egg, 
        Coin, 
        paper, 
        emptybottle, 
        wellbottle, 
        Envelope

    }

    






}
