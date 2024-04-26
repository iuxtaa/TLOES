using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing2 : MonoBehaviour
{
    public Controller Controller;
    public Items[] store;

    public void storeitem(int index)
    {
        bool result = Controller.AddItem(store[index]);
        if (result == true)
        {
            Debug.Log("Stored");

        }

        else
        {
            Debug.Log("Can not be Stored");

        }
    }



}

