using Assets.ArcherChar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryShowUp : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    public GameObject ItemHolder;
    void OnEnable()
    {

        inventory = GameObject.Find("Character").GetComponent<InventoryController>().inventory;
        int counter = 0;
       

        foreach (Item item in inventory.Items)
        {
            
            
            GameObject temp_holder = Instantiate(ItemHolder, transform);
            temp_holder.transform.SetParent(GameObject.Find("Inventory").transform);
            temp_holder.GetComponent<Button>().image.sprite = item.Sprite;
            Vector2 pos = temp_holder.GetComponent<RectTransform>().anchoredPosition;
            pos.x += counter * 120;
            temp_holder.GetComponent<RectTransform>().anchoredPosition = pos;
            counter++;
        }

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnDisable()
    {
        while(transform.childCount > 4)
        {
            DestroyImmediate(transform.GetChild(transform.childCount-1).gameObject);
        }
            
        
    }
}
