using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest", menuName = "Quest System/Collecting Quest")]




public class CollectingQuest : Quest  // Inherits from Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;
    public int requiredAmount;
    public int currentAmount = 0;  // Tracks the amount of the item collected

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectingQuest(GameObject requiredItem, int requiredAmount) : base()
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    void UpdateQuestProgress()
    {
        if (Controller.Instance != null && requiredItem != null)
        {
            
            currentAmount = Controller.Instance.GetItemCount(requiredItem.GetComponent<Items>());
            CheckCompletion();
        }
    }

    void OnEnable()
    {
        
        Controller.Instance.OnItemChanged += UpdateQuestProgress;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        Controller.Instance.OnItemChanged -= UpdateQuestProgress;
    }

    // METHODS
    public bool isReached()
    {
        return currentAmount >= requiredAmount;
    }

    public void addItem(int amount)
    {
        currentAmount += amount;
        CheckCompletion();
    }

    public bool canAddItem(int amount)
    {
        return (currentAmount + amount) <= requiredAmount;
    }





    public override void complete()
    {
        if (!isComplete)
        {
            base.complete();  
            Debug.Log(title + " quest is completed");
            
        }
    }

    // This function checks if the quest requirements are met and marks the quest as complete
    void CheckCompletion()
    {
        if (isReached())
        {
            complete();
        }
    }

    
}