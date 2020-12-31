using Assets.ArcherChar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    /*void OnEnable()
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
*/

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 55f;
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x* itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            x++;
            if (x > 2)
            {
                x = 0;
                y--;
            }
        }
    }
  
}
   

