using Inventory.Container;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public abstract class EquipmentInventoryView : MonoBehaviour
    {
        public InventoryObject inventory;
        public GameObject SlotPrefab;
        protected InventorySlot[] slots;

        private static readonly MouseItem mouseItem = new MouseItem();
        
        public abstract void CreateSlots();

        private void Start()
        {
            CreateSlots();
        }

        public void OnEnter(InventorySlot slot)
        {
            Debug.Log("OnEnter");
        }

        public void OnExit(InventorySlot slot)
        {
            if (mouseItem != null && mouseItem.toSlot != null)
                mouseItem.toSlot = null;
        }

        public void OnDragStart(InventorySlot slot)
        {
            
        }
        
        public void OnDrag(InventorySlot slot, PointerEventData eventData)
        {
            
        }
        
        public void OnDragEnd(InventorySlot slot)
        {
            
        }
    }
}