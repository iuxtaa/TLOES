using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUiItem : MonoBehaviour
{
   //[SerializeField] InventoryUiItem Itemprefab;
    [SerializeField] GameObject Itemprefab;
    [SerializeField] RectTransform contentPanel; 

    List<InventoryUiItem> ListofUIItems = new List<InventoryUiItem>();


    public void InitializeInventoryUi(int InventorySize)


    {
        for (int i = 0; i < InventorySize; i++)
        {


            GameObject Itemprefab = Instantiate(Resources.Load("ItemUI")) as GameObject;
            Itemprefab.SetActive(true);
            ListofUIItems.Add(ListofUIItems[i]);



            
             


           // InventoryUiItem uiItem = Instantiate( Itemprefab, Vector3.zero, Quaternion.identity);

            //uiItem.transform.parent = contentPanel;
           // ListofUIItems.Add(uiItem);



        } }

       public void Show()
        {

            gameObject.SetActive(true);
        }

        public void Hide()
        {

            gameObject.SetActive(false);
        }
    }


