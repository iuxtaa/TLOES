using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // INSTANCE VARIABLES
    private new String name;
    private int currentLocation;

    // CONSTRUCTORS
    public Character(String name)
    {
        SetName(name);
        SetCurrentLocation((int)ScreenEnum.Farm);
    }

    public Character(String name, int currentLocation)
    {
        SetName(name);
        SetCurrentLocation(currentLocation);
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

}
