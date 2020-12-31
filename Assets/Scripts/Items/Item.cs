using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ArcherChar
{
    public class Item 
    {
        public enum ItemType
        {
            Arrow,
            HealthPotion,
            ManaPotion,
            Coin,
            Cookie,
            Apple
        }

        public Sprite GetSprite()
        {
            switch (itemType)
            {
                
                case ItemType.HealthPotion:
                    return ItemAssets.Instance.healtPoitonSprite;
                case ItemType.ManaPotion:
                    return ItemAssets.Instance.manaPotionSprite;
                case ItemType.Cookie:
                    return ItemAssets.Instance.coinSprite; 
                case ItemType.Apple:
                    return ItemAssets.Instance.appleSprite;
               
            }
            return null;
        }

        public ItemType itemType;
        public int amount;
        /*
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
        }*/


    }


}
