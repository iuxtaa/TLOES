using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // INSTANCE VARIABLES
    private new String name;
    // private Dictionary<string, int> inventory; 
    private int currentLocation;
    private int currentQuest;

    // CONSTRUCTORS
    public Character(String name)
    {
        SetName(name);
        SetCurrentLocation((int)ScreenEnum.Farm);
        SetCurrentQuest(0);
    }

    public Character(String name, int currentLocation, int currentQuest)
    {
        SetName(name);
        SetCurrentLocation(currentLocation);
        SetCurrentQuest(currentQuest);
    }

    // METHODS
    public void SetName(String name)
    {
        this.name = name;
    }

    public String GetName()
    {
        return this.name;
    }

    public void SetCurrentLocation(int currentLocation)
    {
        this.currentLocation = currentLocation;
    }

    public int GetCurrentLocation()
    {
        return this.currentLocation;
    }

    public void SetCurrentQuest(int currentQuest)
    {
        this.currentQuest = currentQuest;
    }

    public int GetCurrentQuest()
    {
        return this.currentQuest;
    }
}
