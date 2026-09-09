using Inventory.Item.Flags;
using Inventory.Item;
using Inventory.Item.ToolItems;
using UnityEngine;

namespace Inventory.Item.ToolItem
{
    [CreateAssetMenu(fileName = "New Tool Item", menuName = "Inventory System/Item/Tool Item Object")]
    
    public class ToolItemObject : ItemsObject, IToolUsable
    {
        [field: SerializeField] public ToolCapability Capability { get; set; }
        //Это поле может получить и может переписать
        [field: SerializeField] public int Volume { get; private set; }
        //Это поле может получить и НЕ может переписать
    }
}