using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // INSTANCE VARIABLE
    private int favourability;

    // CONSTRUCTOR
    public Player(string name) : base(name)
    {
    }

    public Player(string name, int favourability, int currentLocation, int currentQuest) : base(name, currentLocation, currentQuest)
    {
        SetFavourability(favourability);
    }

    // METHODS
    public void SetFavourability(int favourability)
    {
        this.favourability = favourability;
    }

    public int GetFavourability()
    {
        return this.favourability;
    }
}
