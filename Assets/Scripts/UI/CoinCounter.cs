using Assets.ArcherChar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    // Start is called before the first frame update
    InventoryController playerInventory;
    Text coinText;
    void OnEnable()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        coinText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        //coinText.text = playerInventory.inventory.Coinsack.ToString();
        
    }
       
}
