using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Controller Controller;
    public Items[] Pickup; 

    public void Pickupitem(int index)
    {
       bool result =  Controller.AddItem(Pickup[index]);
        if (result == true )
        {
            Debug.Log("Picked Item");

            }

        else
        {
            Debug.Log("Can Not pick");

        }
        }



    }





