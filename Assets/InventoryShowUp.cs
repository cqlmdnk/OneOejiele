using Assets.ArcherChar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShowUp : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    
    void Awake()
    {
        inventory = GameObject.Find("Character").GetComponent<InventoryController>().inventory;

    }

    // Update is called once per frame
    void Update()
    {
        int a = 1;
        
    }
}
