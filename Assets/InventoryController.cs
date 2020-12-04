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
    void Start()
    {
        inventory = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        arrow.text = inventory.Arrowsack.ToString();
        coin.text = inventory.Coinsack.ToString();


    }

    void OnCollisionEnter2D(Collision2D col) // all item capture from will be here
    {
        if (col.gameObject.tag.Equals("Collectable"))
        {

            if (col.gameObject.name.Contains("Coin"))
            {
                inventory.Coinsack++;


            }
            else if (col.gameObject.name.Contains("ArrowSack"))
            {

                int arrowTaken = (int)UnityEngine.Random.Range(4.0f, 9.0f);
                inventory.Arrowsack += arrowTaken;

            }
            else // if there are same kind items stack them
            {
                Item item = col.gameObject.GetComponent<Item>();
                item.transform.parent = this.transform;
                inventory.AddItem(item);
               
            }
            Destroy(col.gameObject);
        }

    }
}
