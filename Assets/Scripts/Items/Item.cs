using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ArcherChar
{
    public class Item : MonoBehaviour
    {
        private Sprite sprite;
        private int iteid = 0, price = 0;
        private KeyValuePair<string, int> value;


       
        public int Iteid { get => iteid; set => iteid = value; }
        public Sprite Sprite { get => sprite; set => sprite = value; }
    
        public int Price { get => price; set => price = value; }
        public KeyValuePair<string, int> Value { get => value; set => this.value = value; }

        
        private void Init(Sprite sprite,  int iteid = 0, int price = 0, KeyValuePair<string, int> value = default)
        {
            this.sprite = sprite;
            this.iteid = iteid;
            this.price = price;
            this.value = value;
        }

        private void Start()
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            if (this.name.Contains("Coin"))
            {
                Init(sprite,  0, 1, new KeyValuePair<string, int>("Coin",1));
            }
            else if (this.name.Contains("ArrowSack"))
            {
                Init(sprite,  1, 1, new KeyValuePair<string, int>("Arrow",5));
            }
            else if (this.name.Contains("Cookie"))
            {
                Init(sprite,  2, 10, new KeyValuePair<string, int>("HP and MANA",10));
            }
            else if (this.name.Contains("HealthPotion"))
            {
                Init(sprite,  4, 25, new KeyValuePair<string, int>("HP",35));
            }
            else if (this.name.Contains("ManaPotion"))
            {
                Init(sprite,  6, 25, new KeyValuePair<string, int>("MANA",35));
            }
        }


    }


}
