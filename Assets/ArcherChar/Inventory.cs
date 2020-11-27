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
        private int coinsack, arrowsack;
        public Inventory()
        {
            items = new List<Item>();
        }

        public int Coinsack { get => coinsack; set => coinsack = value; }
        public int Arrowsack { get => arrowsack; set => arrowsack = value; }


        public int GetSizeOfItems() { return items.Count; }
        public void AddItem(Item item)
        {
            items.Add(item);
        }
        public bool RemoveItem(Item item)
        {
            return items.Remove(item);
        }
    }
}
