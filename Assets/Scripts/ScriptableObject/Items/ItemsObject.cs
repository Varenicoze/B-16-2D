using UnityEngine;
using System;
using Inventory.Item.Flags;

namespace Inventory.Item
{
    public abstract class ItemsObject : UnityEngine.ScriptableObject
    {
        public int Id;
        public string Name;
        public Sprite uiDisplay;
        [TextArea(15, 20)] public string Description;
        public int MaxStack;
        public ItemCategory Category;

        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
    }

    [Serializable]
    public class Item
    {
        public int Id;
        public string Name;
        public int MaxStack;

        public Item(ItemsObject item)
        { 
            Id = item.Id;
            Name = item.Name;
            MaxStack = item.MaxStack;
        }
    }
}