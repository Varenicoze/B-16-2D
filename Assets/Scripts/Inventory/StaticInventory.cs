using System;
using Inventory.Container;
using UnityEngine;

namespace Inventory
{
    public class StaticInventory : InventoryView
    {
        private GameObject _staticSlot;
        
        public event Action<InventorySlot> OnItemSelected;
        
        public override void CreateSlots()
        {
            var inventorySlots = inventory.container.Items;

            /*for (int i = 0; i < _staticSlot.Length; i++)
            {
                var obj = transform.GetChild(i).gameObject;
                SlotEventBinder.BindSlotEvent(_staticSlot[i], items[i], this);
                SlotEventBinder.BindClickEvent(_staticSlot[i], items[i], this);
                _staticSlot[i].GetComponent<InventorySlotView>().Bind(items[i], inventory.database);
                inventorySlots[i].Bind(inventory.database);
            }*/
        }

        public /*override*/ void OnLeftClick(InventorySlot slot)
        {
            if(slot.item == null)
                return;
            
            OnItemSelected?.Invoke(slot);
        }
    }
}