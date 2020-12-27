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

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>().inventory;
        int counterX = 0;
        int counterY = 0;

        foreach (Item item in inventory.Items)
        {

            if(counterX > 4)
            {
                counterY++;
                counterX = 0;
            }
            GameObject temp_holder = Instantiate(ItemHolder, transform);
            temp_holder.transform.SetParent(GameObject.Find("Inventory").transform);
            temp_holder.GetComponent<Button>().image.sprite = item.Sprite;
            Vector2 pos = temp_holder.GetComponent<RectTransform>().anchoredPosition;
            pos.x += counterX * 120;
            pos.y += 25 - counterY *130;
            temp_holder.GetComponent<RectTransform>().anchoredPosition = pos;
            temp_holder.GetComponent<Button>().onClick.AddListener(delegate { applyItem(item); }); 
            counterX++;
        }

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnDisable()
    {
        while(transform.childCount > 10)
        {
            DestroyImmediate(transform.GetChild(transform.childCount-1).gameObject);
        }
            
        
    }
    void applyItem(Item item)
    {
        GameObject characterObject = GameObject.FindGameObjectWithTag("Player");
        CharacterController controller = characterObject.GetComponent<CharacterController>();
        applyPotions(controller, item);
        Debug.Log(item.Value);
    }
    void applyPotions(CharacterController charController, Item item)
    {
        if (item.Value.Key.Contains("HP"))
        {
            if (item.Value.Key.Contains("MANA"))
            {

            }
            else
            {
                charController.GetComponent<HealthController>().AddHealth(item.Value.Value);
            }
        }
        else if (item.Value.Key.Contains("MANA"))
        {

        }
    }
}
