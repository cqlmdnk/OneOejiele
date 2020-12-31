using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ArcherChar
{
    public class Inventory 
    {
        private List<Item> items;
        public Inventory()
        {
            Items = new List<Item>();
            AddItem(new Item { itemType = Item.ItemType.Apple, amount = 1 });
            AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });

            AddItem(new Item { itemType = Item.ItemType.Cookie, amount = 1 });
            AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });

        }

       
        public List<Item> Items { get => items; set => items = value; }

        public int GetSizeOfItems() { return Items.Count; }
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        
        public bool RemoveItem(Item item)
        {
            return Items.Remove(item);
        }
        public List<Item> GetItemList()
        {
            return items;
        }
    }
}
