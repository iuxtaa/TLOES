using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;



    public class Testing3 : MonoBehaviour
    {
        public Controller Controller;
        public Items[] discardIndices;  // Array of indices to discard

        public void DiscardItem(int index)
        {
            if (index < 0 || index >= Controller.inventoryItems.Count)
            {
                Debug.Log("Item Discarded");
                return;
            }

            Controller.DiscardItem(index);
        }
    }

