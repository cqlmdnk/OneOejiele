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
            Items = new List<Item>();
        }

        public int Coinsack { get => coinsack; set => coinsack = value; }
        public int Arrowsack { get => arrowsack; set => arrowsack = value; }
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
    }
}
