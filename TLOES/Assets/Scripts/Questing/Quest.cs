using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Unity will serialize these fields so that they will show up in the Inspector
public class Quest
{
    // PRIVATE INSTANCE VARIABLES
    public int questNumber;

    public bool isActive; // boolean for quest status

    public string title;
    public string description;
    public int favourabilityReward;

    // METHODS

    public void complete()
    {
        isActive = false;
        Debug.Log(title + " quest is completed. ");
    }
}
