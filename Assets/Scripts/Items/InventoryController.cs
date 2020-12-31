using Assets.ArcherChar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    public Text arrow, coin;

    private UI_Inventory uiInventory;
    void Start()
    {
        inventory = new Inventory();
        uiInventory = GameObject.Find("Inventory").GetComponent<UI_Inventory>();
        uiInventory.SetInventory(inventory);
    }

    // Update is called once per frame
    void Awake()
    {
       //arrow.text = inventory.Arrowsack.ToString();
       //coin.text = inventory.Coinsack.ToString();


    }

    void OnCollisionEnter2D(Collision2D col) // all item capture from will be here
    {
        /*
         if (col.gameObject.tag.Equals("Collectable"))
        {

            if (col.gameObject.name.Contains("Coin"))
            {
                inventory.IncreaseCoin();


            }
            else if (col.gameObject.name.Contains("ArrowSack"))
            {

                
                inventory.AddArrow();

            }
            else // if there are same kind items stack them
            {
                Item item = col.gameObject.GetComponent<Item>();
                item.transform.parent = this.transform;
                inventory.AddItem(item);
               
            }
            Destroy(col.gameObject);
        }*/

    }
}
