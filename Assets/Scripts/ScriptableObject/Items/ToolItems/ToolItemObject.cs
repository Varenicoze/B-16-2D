using Inventory.Items;
using Inventory.Items.Flags;
using Inventory.Items.ToolItems;
using UnityEngine;

namespace Inventory.Items.ToolItem
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