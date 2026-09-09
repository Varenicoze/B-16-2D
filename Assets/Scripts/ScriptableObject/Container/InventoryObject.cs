using UnityEngine;
using Inventory.Database;
using System;
using Inventory.Item;

namespace Inventory.Container
{
    [CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public ItemDatabaseObject database; //ссылка на базу данных
        public Inventory container;//представляет из себя весь инвентарь, содержит в себе кол-во слотов, которые могут быть

        public void AddItem(Item.Item item, int amount)
        {
            int remaining = amount;
            
            for (int i = 0; i < container.Items.Length; i++)
            {
                if(amount <= 0)
                    return;//проверка на дурака, если забыли добавить в мир предмет
                
                if(container.Items[i].ID != item.Id)
                    continue;

                int space = container.Items[i].item.MaxStack - container.Items[i].amount;
                if(space <= 0)
                    continue;
                
                int toAdd = Math.Min(space, remaining);
                container.Items[i].AddAmount(toAdd);
                remaining -= toAdd;
            }

            while (remaining > 0)
            {
                int toAdd = Math.Min(item.MaxStack, remaining);
                bool placed = SetEmptySlot(item, toAdd);

                if(!placed)
                    return;
                remaining -= toAdd;
            }
        }

        public void RemoveItem(InventorySlot slot)
        {
            slot.UpdateSlot(-1, 0, database);
        }

        public void MoveItem(InventorySlot toSlot, InventorySlot fromSlot)
        {
            if (ReferenceEquals(toSlot, fromSlot))
                return;

            if (toSlot.ID == fromSlot.ID)
            {
                int amount = toSlot.amount + fromSlot.amount;
                
                if (toSlot.item.MaxStack > amount)
                {
                    toSlot.AddAmount(fromSlot.amount);
                    RemoveItem(fromSlot);
                    return;
                }
                
                int space = toSlot.item.MaxStack - toSlot.amount;//то, что должно остаться в ячейке
                toSlot.AddAmount(space);//кол-во,которое нужно добавить в
                fromSlot.AddAmount(-space);//кол-во,которое нужно забрать из
                return;
            }
            
            InventorySlot temp = new InventorySlot(fromSlot.ID, fromSlot.amount);
            
            fromSlot.UpdateSlot(toSlot.ID, toSlot.amount, database);
            toSlot.UpdateSlot(temp.ID, temp.amount, database);
        }

        [ContextMenu("Clear Slots")]
        public void ClearSlots()
        {
            foreach (var slot in container.Items)
            {
                slot.UpdateSlot(-1, 0, database);
            }
        }
        
        
        private bool SetEmptySlot(Item.Item item, int value)
        {
            foreach (var slot in container.Items)
            {
                if (slot.ID <= -1)
                {
                    slot.UpdateSlot(item.Id, value, database);
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class Inventory
    {
        public InventorySlot[] Items;
        
    }
    
    [Serializable]
    public class InventorySlot
    {
        [field: NonSerialized] public event Action OnChanged;
        
        [NonSerialized] private Item.Item _cachedItem;
        public Item.Item item => _cachedItem;
        
        public int ID = -1;
        public int amount;
        public object _inventory;

        public InventorySlot()//слот может принимать пустое значение
        {
            ID = -1;
            amount = 0;
        }
        
        public InventorySlot(int id, int amount)//слот может принимать значение(какое-нибудь)
        {
            ID = id;
            this.amount = amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }

        public void UpdateSlot(int id, int value, ItemDatabaseObject database)
        {
            ID = id;
            amount = value;
            RefreshCache(database);
            OnChanged?.Invoke();
        }
        
        
        private bool CanPlaceInSlot(ItemsObject item)
        {
            if (ID <= -1 && amount <= 0)
                return true;

            return false;
        }

        public void Bind(ItemDatabaseObject database)
        {
            RefreshCache(database);
        }
        
        private void RefreshCache(ItemDatabaseObject database)
        {
            _cachedItem = ID >= 0 ? database.GetItem[ID].CreateItem() : null;
        }
    }
}