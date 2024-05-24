using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI money;
    private CollectableItems collect;

    public void Awake()
    {
        collect = GetComponent<CollectableItems>();
    }

    public void Update()
    {
        moneyUI();
    }

    public void moneyUI()
    {
        if (money != null)
        {
            money.text = collect.changingnum.ToString();
        }
    }
}
