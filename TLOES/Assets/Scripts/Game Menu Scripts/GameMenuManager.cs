using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject newPlayerPanel;
    [SerializeField] private GameObject oldPlayerPanel;
    private int menuChoice = StartingPanelManager.menuChoice;

    public void Awake() 
    {
        if(menuChoice == (int)StartingMenuChoice.Login) 
        {
            oldPlayerPanel.SetActive(true);
            
        }
        if(menuChoice == (int)StartingMenuChoice.Signup)
        {
            newPlayerPanel.SetActive(true);
        }
    }
}


