using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


[CreateAssetMenu (menuName ="Items")]
public class Items : ScriptableObject
{
    [Header("Type")]
    public ItemType type;

    [Header("UI")]
    public bool stackable = true;

    [Header("Appearance")]
    public Sprite Image;

    [Header("Amount")]
    public int count = 1; // Add a count property to track the number of stacked items

    public enum ItemType
    {
        Egg,
        Paper,
        EmptyBottle,
        GoldCoin,
        SilverCoin,
        WaterBottle,
        Envelope
    }

    public enum ActionType
    {
        None
    }
}
